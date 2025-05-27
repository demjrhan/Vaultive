import { showMovieDetail } from './detailView.js';

export async function renderMovies(movies) {
  const popupContentBox = document.querySelector(
    '.movies-popup-container .content-box-moviePage',
  );
  popupContentBox.innerHTML = '';

  const genreMap = new Map();

  movies.forEach((movie) => {
    movie.genres?.forEach((genre) => {
      if (!genreMap.has(genre)) {
        genreMap.set(genre, []);
      }
      genreMap.get(genre).push(movie);
    });
  });

  const shuffledGenres = [...genreMap.keys()].sort(() => Math.random() - 0.5);

  shuffledGenres.forEach((genreName) => {
    const genreMovies = genreMap.get(genreName);
    const genreContainer = document.createElement('div');
    genreContainer.className = 'genre-container';

    genreContainer.innerHTML = `
        <div class="title-buttons-wrapper">
          <h2 class="genre-title">${genreName}</h2>
          <div class="scroll-buttons">
            <button class="scroll-button left">&#10094;</button>
            <button class="scroll-button right">&#10095;</button>
          </div>
        </div>
        <div class="movies">
          <div class="movie-posters"></div>
        </div>
      `;

    const postersContainer = genreContainer.querySelector('.movie-posters');

    genreMovies.forEach((movie) => {
      const posterImage = movie.mediaContent?.posterImageName
        ? `../public/img/${movie.mediaContent.posterImageName}.png`
        : '../public/img/default-poster.png';

      const img = document.createElement('img');
      img.src = posterImage;
      img.alt = movie.mediaContent?.title ?? 'Untitled';
      img.addEventListener('click', () => showMovieDetail(movie, 'movies'));
      postersContainer.appendChild(img);
    });

    popupContentBox.appendChild(genreContainer);
  });
}
