import { details, featuredMovie } from './movieData.js';

const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');
const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const detailDescription = document.querySelector('.movie-text-description-detail');
const showcase = mainContainer.querySelector('.showcase-container');

export function showMovieDetail(movie) {
  const detail = details.find((d) => d.id === movie.id);
  detailImage.innerHTML = `<img src=${movie.src} alt=${movie.alt}>`;
  detailTitle.innerHTML = detail.title;
  detailDescription.innerHTML = detail.description;

  detailImage.style.backgroundImage = `
    linear-gradient(to bottom, rgba(0,0,0, 0) 0%, rgba(0,0,0, 1) 100%),
    ${detail.background}
  `;

  detailContainer.style.display = 'flex';
  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  showcase.style.backgroundImage = `linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
                                    url(${featuredMovie.backgroundHolder})`;
  document.body.classList.add('detail-view-open');

}

export function closeDetailOnClick() {
  window.onclick = function (event) {
    if (event.target === detailContainer) {
      closeDetailView();
    }
  };
}

export function closeDetailOnEscape() {
  window.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      closeDetailView();
      showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
      showcase.style.backgroundRepeat = 'no-repeat';
      showcase.style.backgroundSize = 'cover';
      showcase.style.backgroundPosition = 'center';
    }
  });
}

function closeDetailView() {
  detailContainer.style.display = 'none';
  mainContainer.style.filter = 'grayscale(0%) blur(0px)';
  document.body.classList.remove('detail-view-open');
}
