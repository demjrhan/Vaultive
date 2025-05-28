import { showMovieDetail } from './detailView.js';

export function renderRecommendations(movies) {
  const movieCardsContainer = document.querySelector('.movie-cards');
  movieCardsContainer.innerHTML = '';

  function getMoviesPerPage() {
    const width = window.innerWidth;

    switch (true) {
      case width > 1800:
        return 5;
      case width > 1300:
        return 4;
      case width > 1200:
        return 5;
      case width > 900:
        return 4;
      default:
        return 3;
    }
  }

  const visibleMovies = movies.slice(0, getMoviesPerPage());

  visibleMovies.forEach((movie) => {
    const img = document.createElement('img');
    img.className = 'movie-cards-img';
    img.src = `../public/img/${movie.mediaContent.posterImageName}.png`;
    img.alt = movie.mediaContent?.title;
    img.addEventListener('click', () => showMovieDetail(movie, 'home'));
    movieCardsContainer.appendChild(img);
  });
}
