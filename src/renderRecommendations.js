import { showMovieDetail } from './detailView.js';

let currentIndex = 0;

export function renderRecommendations(recommendations) {
  const movieCardsContainer = document.querySelector('.movie-cards');
  const nextButton = document.querySelector('.next-button');
  const prevButton = document.querySelector('.prev-button');

  // Determine movies per page based on current screen width
  function getMoviesPerPage() {
    const width = window.innerWidth;
    if (width > 1280) return 5;
    if (width > 1000) return 4;
    return 3;
  }

  function updateMovies() {
    movieCardsContainer.innerHTML = '';

    const MOVIES_PER_PAGE = getMoviesPerPage();
    const endIndex = currentIndex + MOVIES_PER_PAGE;
    const visibleMovies = recommendations.slice(currentIndex, endIndex);

    visibleMovies.forEach(movie => {
      const img = document.createElement('img');
      img.className = 'movie-cards-img';
      img.src = movie.src;
      img.alt = movie.alt;
      img.addEventListener('click', () => showMovieDetail(movie));
      movieCardsContainer.appendChild(img);
    });

    prevButton.style.display = currentIndex === 0 ? 'none' : 'flex';
    nextButton.style.display = endIndex >= recommendations.length ? 'none' : 'flex';

    if (visibleMovies.length < MOVIES_PER_PAGE) {
      movieCardsContainer.style.justifyContent = 'flex-start';
    } else if (currentIndex === 0 && visibleMovies.length <= 3) {
      movieCardsContainer.style.justifyContent = 'center';
    } else {
      movieCardsContainer.style.justifyContent = 'center';
    }
  }

  nextButton.onclick = () => {
    const MOVIES_PER_PAGE = getMoviesPerPage();
    if (currentIndex + MOVIES_PER_PAGE < recommendations.length) {
      currentIndex += MOVIES_PER_PAGE;
      updateMovies();
    }
  };

  prevButton.onclick = () => {
    const MOVIES_PER_PAGE = getMoviesPerPage();
    if (currentIndex - MOVIES_PER_PAGE >= 0) {
      currentIndex -= MOVIES_PER_PAGE;
      updateMovies();
    } else {
      currentIndex = 0;
      updateMovies();
    }
  };

  // Ensure responsiveness on window resize
  window.addEventListener('resize', () => {
    currentIndex = 0; // reset to first page when resizing
    updateMovies();
  });

  updateMovies();
}
