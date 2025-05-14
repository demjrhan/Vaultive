import { renderFeaturedMovie } from './renderFeatured.js';
import { renderRecommendations } from './renderRecommendations.js';
import { closeDetailOnEscape } from './detailView.js';
import { details, featuredMovie, recommendations } from './movieData.js';
import { openMoviesPopup } from './moviesPage.js';
import { renderMovies } from './renderMovies.js';

document.addEventListener('DOMContentLoaded', () => {
  renderFeaturedMovie(featuredMovie);
  renderRecommendations(recommendations);
  closeDetailOnEscape();

  window.addEventListener('resize', () => {
    renderRecommendations(recommendations);
  });

  const moviesButton = document.getElementById('movies-link');
  moviesButton.addEventListener('click', () => {
    openMoviesPopup();
    renderMovies(details);
  });
});
