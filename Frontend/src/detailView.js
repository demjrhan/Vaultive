import { featuredMovie } from './movieData.js';

const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');
const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const detailDescription = document.querySelector(
  '.movie-text-description-detail',
);
const showcase = mainContainer.querySelector('.showcase-container');
const moviesPopupContainer = document.querySelector('.movies-popup-container');

let detailOpenedFrom = 'home';

export function showMovieDetail(movie, from = 'home') {
  detailOpenedFrom = from;

  detailImage.innerHTML = `<img src="../public/img/${movie.mediaContent.posterImage}.png" alt="${movie.mediaContent?.title}">`;
  detailTitle.innerHTML = movie.mediaContent?.title ?? 'Untitled';
  detailDescription.innerHTML =
    movie.mediaContent?.description ?? 'No description available.';


  detailImage.style.backgroundImage =`
    linear-gradient(to bottom, rgba(0,0,0, 0) 0%, rgba(0,0,0, 1) 100%),
  url(../public/img/${movie.mediaContent?.backgroundImage}.png)`
  ;

  detailContainer.style.display = 'flex';

  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  showcase.style.backgroundImage = `linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
                                    url(${featuredMovie.backgroundHolder})`;

  if (from === 'movies') {
    moviesPopupContainer.style.filter = 'grayscale(100%) blur(5px)';
    moviesPopupContainer.classList.add('overlay-disabled');
  }


  showcase.classList.add('overlay-disabled');
}

export function closeDetailOnEscape() {
  window.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && detailContainer.style.display === 'flex') {
      closeDetailView();

      if (detailOpenedFrom === 'home') {
        showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
        showcase.style.backgroundRepeat = 'no-repeat';
        showcase.style.backgroundSize = 'cover';
        showcase.style.backgroundPosition = 'center';
      }
    }
  });
}

function closeDetailView() {
  detailContainer.style.display = 'none';

  if (detailOpenedFrom === 'movies') {
    moviesPopupContainer.style.filter = 'none';
    moviesPopupContainer.classList.remove('overlay-disabled');
  } else {
    mainContainer.style.filter = 'none';
  }


  showcase.classList.remove('overlay-disabled');
}
