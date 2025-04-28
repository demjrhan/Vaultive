const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');
import { featuredMovie, recommendations, details } from './movies.js';


const starContainer = document.querySelector('.star-movies-container');
starContainer.innerHTML = `
  <h3 class="star-movie-publisher">${featuredMovie.publisher}</h3>
  <h1>${featuredMovie.title}</h1>
  <img class="star-movie-img" src="${featuredMovie.imageSrc}" alt="${featuredMovie.title}">
`;

const movieCardsContainer = document.querySelector('.movie-cards');
movieCardsContainer.innerHTML = '';

const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const detailDescription = document.querySelector(
  '.movie-text-description-detail',
);
const showcase = mainContainer.querySelector('.showcase-container');

recommendations.forEach((movie) => {
  const img = document.createElement('img');
  img.className = 'movie-cards-img';
  img.src = movie.src;
  img.alt = movie.alt;

  img.addEventListener('click', () => {
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
  });

  movieCardsContainer.appendChild(img);
});

window.onclick = function (event) {
  if (event.target === detailContainer) {
    detailContainer.style.display = 'none';
    mainContainer.style.filter = 'grayscale(0%) blur(0px)';
  }
};

window.addEventListener('keydown', (e) => {
  if (e.key === 'Escape') {
    detailContainer.style.display = 'none';
    mainContainer.style.filter = 'grayscale(0%) blur(0px)';
    showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
    showcase.style.backgroundRepeat = 'no-repeat';
    showcase.style.backgroundSize = 'cover';
    showcase.style.backgroundPosition = 'center';
  }
});
