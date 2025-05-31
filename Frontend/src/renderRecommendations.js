import { showMovieDetail } from './detailView.js';

export function renderRecommendations(movies) {
  const movieCardsContainer = document.querySelector('.movie-cards');
  movieCardsContainer.innerHTML = '';

    /* Current movies variable holds every movie in database, depending on the amount of movies we get from the
    getMoviesPerPage() we show user that amount by slicing the array*/
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

/* Returning the amount of the movies will be shown in recommendations div depending on screen size */
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