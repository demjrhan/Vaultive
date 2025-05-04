import { showMovieDetail } from './detailView.js';

export function renderRecommendations(recommendations) {
  const movieCardsContainer = document.querySelector('.movie-cards');
  movieCardsContainer.innerHTML = '';

  recommendations.forEach((movie) => {
    const img = document.createElement('img');
    img.className = 'movie-cards-img';
    img.src = movie.src;
    img.alt = movie.alt;

    img.addEventListener('click', () => showMovieDetail(movie));

    movieCardsContainer.appendChild(img);
  });
}
