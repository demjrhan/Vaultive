import { showMovieDetail } from './detailView.js';

const API_BASE_URL = 'http://localhost:5034/Vaultive';

export async function renderMovies() {
  const popupContentBox = document.querySelector('.movies-popup-container .content-box');
  popupContentBox.innerHTML = '';

  try {
    const response = await fetch(`${API_BASE_URL}/GetAllMovies`);
    if (!response.ok) throw new Error('Failed to fetch movies');
    const movies = await response.json();

    // Group movies by genre
    const genreMap = new Map();

    movies.forEach(movie => {
      movie.genres?.forEach(genre => {
        if (!genreMap.has(genre)) {
          genreMap.set(genre, []);
        }
        genreMap.get(genre).push(movie);
      });
    });

    // Shuffle genre names
    const shuffledGenres = [...genreMap.keys()]
      .sort(() => Math.random() - 0.5); // Simple shuffle

    // Render genres in random order
    shuffledGenres.forEach(genreName => {
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

      genreMovies.forEach(movie => {
        const img = document.createElement('img');
        img.src = `../public/img/${movie.mediaContent.posterImage}.png`;
        img.alt = movie.mediaContent?.title ?? 'Untitled';
        img.addEventListener('click', () => showMovieDetail(movie, 'movies'));
        postersContainer.appendChild(img);
      });

      popupContentBox.appendChild(genreContainer);
    });

  } catch (error) {
    console.error('Error loading movies:', error);
    popupContentBox.innerHTML = '<p class="error-message">Failed to load movies.</p>';
  }
}
