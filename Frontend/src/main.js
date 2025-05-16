import { renderFeaturedMovie } from './renderFeatured.js';
import { renderRecommendations } from './renderRecommendations.js';
import { closeDetailOnEscape } from './detailView.js';
import { featuredMovie} from './movieData.js';
import { createNavigationBar, openMoviesPopup } from './moviesPage.js';
import { renderMovies } from './renderMovies.js';

document.addEventListener('DOMContentLoaded', () => {
  renderFeaturedMovie(featuredMovie);
  renderRecommendations();
  closeDetailOnEscape();
  window.addEventListener('resize', () => {
    renderRecommendations();
  });

  const moviesButton = document.getElementById('movies-link');
  moviesButton.addEventListener('click', () => {
    openMoviesPopup();
    createNavigationBar();
    renderMovies();
  });
});
