import { renderFeaturedMovie } from './renderFeatured.js';
import { renderRecommendations } from './renderRecommendations.js';
import { closeDetailOnClick, closeDetailOnEscape } from './detailView.js';
import { featuredMovie, recommendations } from './movieData.js';

document.addEventListener('DOMContentLoaded', () => {
  renderFeaturedMovie(featuredMovie);
  renderRecommendations(recommendations);
  closeDetailOnClick();
  closeDetailOnEscape();

  window.addEventListener('resize', () => {
    renderRecommendations(recommendations);
  });
});
