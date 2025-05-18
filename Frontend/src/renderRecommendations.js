import { showMovieDetail } from './detailView.js';



export async function renderRecommendations() {
  const movieCardsContainer = document.querySelector('.movie-cards');
  movieCardsContainer.innerHTML = '';

  const API_BASE_URL = 'http://localhost:5034/api/Movie';
  try {
    const response = await fetch(`${API_BASE_URL}/All`);
    if (!response.ok) throw new Error('Failed to fetch recommendations');
    const movies = await response.json();
    console.log(movies);

    function getMoviesPerPage() {
      const width = window.innerWidth;

      switch (true) {
        case width > 1800: return 5;
        case width > 1300: return 4;
        case width > 1200: return 5;
        case width > 900: return 4;
        default: return 3;
      }
    }



    const visibleMovies = movies.slice(0,getMoviesPerPage());

    visibleMovies.forEach(movie => {
      const img = document.createElement('img');
      img.className = 'movie-cards-img';
      img.src = `../public/img/${movie.mediaContent.posterImageName}.png`;
      img.alt = movie.mediaContent?.title;
      img.addEventListener('click', () => showMovieDetail(movie, 'home'));
      movieCardsContainer.appendChild(img);
    });
  } catch (error) {
    console.error('Error loading recommended movies:', error);
    movieCardsContainer.innerHTML = '<p class="error-message">Failed to load recommendations.</p>';
  }
}
