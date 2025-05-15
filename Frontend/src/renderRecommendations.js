import { showMovieDetail } from './detailView.js';

let currentIndex = 0;

export async function renderRecommendations() {
  const movieCardsContainer = document.querySelector('.movie-cards');
  movieCardsContainer.innerHTML = '';

  const API_BASE_URL = 'http://localhost:5034/Vaultive';
  try {
    const response = await fetch(`${API_BASE_URL}/GetAllMovies`);
    if (!response.ok) throw new Error('Failed to fetch recommendations');
    const movies = await response.json();

    function getMoviesPerPage() {
      const width = window.innerWidth;
      if (width > 1800) return 5;
      if (width > 1400) return 4;
      if (width > 1300) return 4;
      if (width > 1200) return 5;
      if (width > 900) return 4;
      return 3;
    }

    const endIndex = currentIndex + getMoviesPerPage();
    const visibleMovies = movies.slice(currentIndex, endIndex);

    visibleMovies.forEach(movie => {
      const img = document.createElement('img');
      img.className = 'movie-cards-img';
      img.src = 'public/img/placeholder.png';
      img.alt = movie.MediaContent?.Title;
      img.addEventListener('click', () => showMovieDetail(movie, 'home'));
      movieCardsContainer.appendChild(img);
    });
  } catch (error) {
    console.error('Error loading recommended movies:', error);
    movieCardsContainer.innerHTML = '<p class="error-message">Failed to load recommendations.</p>';
  }
}
