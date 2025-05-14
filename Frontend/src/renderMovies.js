import { showMovieDetail } from './detailView.js';

export function renderMovies(details) {
  const popupContentBox = document.querySelector('.movies-popup-container .content-box');
  popupContentBox.innerHTML = ''; // Clear previous genres

  // Step 1: Collect unique genres
  const genreMap = new Map();
  details.forEach(movie => {
    movie.genres.forEach(genre => {
      if (!genreMap.has(genre)) {
        genreMap.set(genre, []);
      }
      genreMap.get(genre).push(movie);
    });
  });

  // Step 2: Render each genre section
  genreMap.forEach((movies, genre) => {
    const genreContainer = document.createElement('div');
    genreContainer.className = 'genre-container';

    genreContainer.innerHTML = `
      <div class="title-buttons-wrapper">
        <h2 class="genre-title">${genre}</h2>
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

    movies.forEach(movie => {
      const img = document.createElement('img');
      img.src = movie.src;
      img.alt = movie.alt;
      img.addEventListener('click', () => showMovieDetail(movie, 'movies'));
      postersContainer.appendChild(img);
    });

    popupContentBox.appendChild(genreContainer);
  });
}
