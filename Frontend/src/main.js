import { renderStar } from './renderStar.js';
import { renderRecommendations } from './renderRecommendations.js';
import { closeDetailOnEscape } from './detailView.js';
import { featuredMovie, fetchMovieData, searchTextListener } from './movieData.js';
import { createNavigationBarMoviePage, openMoviesPopup } from './moviesPage.js';
import { renderMovies } from './renderMovies.js';
import { createNavigationBarStreamingService, openStreamingPopup } from './streamingServicePage.js';
import { renderStreamingServices } from './renderStreamingServices.js';


document.addEventListener('DOMContentLoaded', async () => {

  const movies = await fetchMovieData();
  renderStar(featuredMovie);
  renderRecommendations(movies);
  closeDetailOnEscape();
  window.addEventListener('resize', () => {
    renderRecommendations(movies);
  });

  const moviesButton = document.getElementById('movies-link');
  moviesButton.addEventListener('click', () => {
    openMoviesPopup();
    createNavigationBarMoviePage();
    renderMovies(movies);

    setTimeout(() => {
      searchTextListener();
    }, 0);
  });


  const seriesButton = document.getElementById('streaming-link');
  seriesButton.addEventListener('click', () => {
    openStreamingPopup();
    createNavigationBarStreamingService();
    renderStreamingServices(movies);
  });
});
