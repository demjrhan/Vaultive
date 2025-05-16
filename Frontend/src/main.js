import { renderFeaturedMovie } from './renderFeatured.js';
import { renderRecommendations } from './renderRecommendations.js';
import { closeDetailOnEscape } from './detailView.js';
import { featuredMovie} from './movieData.js';
import { createNavigationBarMoviePage, openMoviesPopup } from './moviesPage.js';
import { renderMovies } from './renderMovies.js';
import { createNavigationBarStreamingService, openStreamingPopup } from './streamingServicePage.js';
import { renderStreamingServices } from './renderStreamingServices.js';


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
    createNavigationBarMoviePage();
    renderMovies();
  });

  const seriesButton = document.getElementById('streaming-link');
  seriesButton.addEventListener('click', () => {
    openStreamingPopup();
    createNavigationBarStreamingService();
    renderStreamingServices();
  });
});
