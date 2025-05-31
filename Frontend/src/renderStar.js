export function renderStar(movie) {
  const starContainer = document.querySelector('.star-movie-container');
  starContainer.innerHTML = `
    <h3 class="star-movie-publisher">${movie.publisher}</h3>
    <h1>${movie.title}</h1>
    <img class="star-movie-img" src="${movie.imageSrc}" alt="${movie.title}">
  `;

  const showcaseBackground = document.querySelector('.showcase-video');
  showcaseBackground.innerHTML = `
    <iframe
      class="showcase-video"
      src="https://www.youtube.com/embed/${movie.trailerURL}?autoplay=1&mute=1&controls=0&loop=1&playlist=${movie.trailerURL}"
      allow="autoplay; encrypted-media"
    ></iframe>
  `;
}
