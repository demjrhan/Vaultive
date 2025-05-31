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

  /* Used spread operator ... in order to get only the genre names.*/
  const shuffledGenres = [...genreMap.keys()].sort(() => Math.random() - 0.5);

  /* Now the array more or less looks like this ["Action" ["Movie", "Movie"]]
  * I created container for each genre names and adding all movie posters as image. And all
  * images has eventListener for click. Clicking will bring details. */
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
    const leftButton = genreContainer.querySelector('.scroll-button.left');
    const rightButton = genreContainer.querySelector('.scroll-button.right');

    leftButton.addEventListener('click', () => {
      postersContainer.scrollBy({ left: -300, behavior: 'smooth' });
    });

    rightButton.addEventListener('click', () => {
      postersContainer.scrollBy({ left: 300, behavior: 'smooth' });
    });


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
