import { featuredMovie } from './movieData.js';

const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');
const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const platformLinksDetail = document.querySelector('.platform-links-detail');
const detailDescription = document.querySelector(
  '.movie-text-description-detail',
);
const showcase = mainContainer.querySelector('.showcase-container');
const moviesPopupContainer = document.querySelector('.movies-popup-container');

let detailOpenedFrom = 'home';

export function showMovieDetail(movie, from = 'home') {
  detailOpenedFrom = from;


  const posterImage = movie.mediaContent?.posterImage
    ? `../public/img/${movie.mediaContent.posterImage}.png`
    : '../public/img/default-poster.png';

  detailImage.innerHTML = `
  <img src="${posterImage}" alt="${movie.mediaContent?.title}">
  <div class="detail-button-group">
    <button class="review-button">Give Review</button>
  </div>
      
 
`;

  detailTitle.innerHTML = movie.mediaContent?.title ?? 'Untitled';
  detailDescription.innerHTML =
    movie.mediaContent?.description ?? 'No description available.';


  const backgroundImage = movie.mediaContent?.backgroundImage
    ? `../public/img/${movie.mediaContent.backgroundImage}.png`
    : '../public/img/default-background.png';

  detailImage.style.backgroundImage = `
  linear-gradient(to bottom, rgba(0,0,0, 0) 0%, rgba(0,0,0, 1) 100%),
  url(${backgroundImage})
`;

  platformLinksDetail.innerHTML = movie.mediaContent?.streamingServices?.map(s =>
    `<img src="../public/img/streamers/${s.logoImage}.png" alt="${s.name}">`
  ).join('') ?? '';


  detailContainer.style.display = 'flex';

  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  showcase.style.backgroundImage = `linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
                                    url(${featuredMovie.backgroundHolder})`;

  if (from === 'movies') {
    moviesPopupContainer.style.filter = 'grayscale(100%) blur(5px)';
    moviesPopupContainer.classList.add('overlay-disabled');
  }


  const scrollY = window.scrollY;
  document.body.style.position = 'fixed';
  document.body.style.top = `-${scrollY}px`;
  document.body.style.width = '100%';
  detailContainer.style.display = 'flex';

  showcase.classList.add('overlay-disabled');
}

export function closeDetailOnEscape() {
  window.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && detailContainer.style.display === 'flex') {
      closeDetailView();

      const scrollY = document.body.style.top;
      document.body.style.position = '';
      document.body.style.top = '';
      document.body.style.width = '';
      window.scrollTo(0, parseInt(scrollY || '0') * -1);
      detailContainer.style.display = 'none';

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
