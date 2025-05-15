import { showMovieDetail } from './detailView.js';

const API_BASE_URL = 'http://localhost:5034/Vaultive';

export async function renderMovies() {
  const popupContentBox = document.querySelector('.movies-popup-container .content-box');
  popupContentBox.innerHTML = ''; // Clear previous genres

  try {
    const genre = 'Action';
    const response = await fetch(`${API_BASE_URL}/GetMoviesWithGivenGenre/${genre}`);
    if (!response.ok) {
      throw new Error('Failed to fetch movies');
    }

    const movies = await response.json();
    console.log('Fetched movies:', movies);

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
      img.src = '../public/img/avengers-poster.png';
      img.alt = movie.MediaContent?.Title ?? 'Movie Poster';
      img.addEventListener('click', () => showMovieDetail(movie, 'movies'));
      postersContainer.appendChild(img);
    });

    popupContentBox.appendChild(genreContainer);
  } catch (error) {
    console.error('Error loading movies:', error);
    popupContentBox.innerHTML = '<p class="error-message">Failed to load Action movies.</p>';
  }
}
