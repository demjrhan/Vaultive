import { showMovieDetail } from './detailView.js';

let currentIndex = 0;

export function renderRecommendations(recommendations) {
  const movieCardsContainer = document.querySelector('.movie-cards');
  const nextButton = document.querySelector('.next-button');
  const prevButton = document.querySelector('.prev-button');

  function getMoviesPerPage() {
    const width = window.innerWidth;
    if (width > 1700) return 5;
    if (width > 1300) return 4;
    return 3;
  }

  function updateMovies(direction = 'next') {
    const MOVIES_PER_PAGE = getMoviesPerPage();

    movieCardsContainer.className = 'movie-cards';

    movieCardsContainer.classList.add(direction === 'next' ? 'slide-out-left' : 'slide-out-right');

    setTimeout(() => {
      movieCardsContainer.innerHTML = '';

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

      movieCardsContainer.className = 'movie-cards';
      movieCardsContainer.classList.add(direction === 'next' ? 'slide-in-right' : 'slide-in-left');
    }, 250);
  }


  nextButton.onclick = () => {
    const MOVIES_PER_PAGE = getMoviesPerPage();
    if (currentIndex + MOVIES_PER_PAGE < recommendations.length) {
      currentIndex += MOVIES_PER_PAGE;
      updateMovies('next');
    }
  };

  prevButton.onclick = () => {
    const MOVIES_PER_PAGE = getMoviesPerPage();
    if (currentIndex - MOVIES_PER_PAGE >= 0) {
      currentIndex -= MOVIES_PER_PAGE;
      updateMovies('prev');
    } else {
      currentIndex = 0;
      updateMovies('prev');
    }
  };


  window.addEventListener('resize', () => {
    currentIndex = 0;
    updateMovies();
  });

  updateMovies();
}
