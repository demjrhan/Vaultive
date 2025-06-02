const moviesPopupContainer = document.querySelector('.movies-popup-container');
const mainContainer = document.querySelector('.main-container');
const showcase = mainContainer.querySelector('.showcase-container');
const showcaseVideo = showcase.querySelector('.showcase-video');
const navigationBar = document.querySelector('.navigation-bar-moviePage');

export function openMoviesPopup() {
  showcaseVideo.style.display = 'none';


  moviesPopupContainer.style.display = 'flex';
  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  document.body.classList.add('detail-view-open');
}

function closeMoviesPopup() {
  showcaseVideo.style.display = '';


  moviesPopupContainer.style.display = 'none';
  mainContainer.style.filter = 'none';
  document.body.classList.remove('detail-view-open');
}

export function createNavigationBarMoviePage() {
  navigationBar.innerHTML = `
    <div class="home-button" id="close-movies-popup">
          <img src="../public/icons/home.png" alt="home">
    </div>`;

  document
    .getElementById('close-movies-popup')
    ?.addEventListener('click', () => {
      closeMoviesPopup();
    });
}
