import { showMovieDetail } from './detailView.js';

let currentIndex = 0;

export function renderRecommendations(recommendations) {
  const movieCardsContainer = document.querySelector('.movie-cards');

  movieCardsContainer.innerHTML = '';

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
  let visibleMovies = recommendations.slice(currentIndex, endIndex);

  visibleMovies.forEach(movie => {
    const img = document.createElement('img');
    img.className = 'movie-cards-img';
    img.src = movie.src;
    img.alt = movie.alt;
    img.addEventListener('click', () => showMovieDetail(movie));
    movieCardsContainer.appendChild(img);
  });
}
