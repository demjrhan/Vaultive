import { featuredMovie } from './movieData.js';

const moviesPopupContainer = document.querySelector('.streaming-popup-container');
const mainContainer = document.querySelector('.main-container');
const showcase = mainContainer.querySelector('.showcase-container');
const navigationBar = document.querySelector('.navigation-bar-streaming');

export function openStreamingPopup() {
  moviesPopupContainer.style.display = 'flex';
  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  showcase.style.backgroundImage = `linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
                                    url(${featuredMovie.backgroundHolder})`;
  document.body.classList.add('detail-view-open');
}

function closeStreamingServicePopup() {
  moviesPopupContainer.style.display = 'none';
  mainContainer.style.filter = 'none';
  document.body.classList.remove('detail-view-open');
}
export function createNavigationBarStreamingService() {
  navigationBar.innerHTML = `
    <div class="home-button" id="close-streaming-popup">
          <img src="../public/icons/home.png" alt="home">
</div>
  `;

  document.getElementById('close-streaming-popup')?.addEventListener('click', () => {
    closeStreamingServicePopup();
    showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
    showcase.style.backgroundRepeat = 'no-repeat';
    showcase.style.backgroundSize = 'cover';
    showcase.style.backgroundPosition = 'center';
  });
}



