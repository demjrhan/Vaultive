export function renderStar(movie) {
  const starContainer = document.querySelector('.star-movie-container');
  starContainer.innerHTML = `
    <h3 class="star-movie-publisher">${movie.publisher}</h3>
    <h1>${movie.title}</h1>
    <img class="star-movie-img" src="${movie.imageSrc}" alt="${movie.title}">
  `;
}
