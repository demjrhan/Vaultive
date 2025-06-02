
const reviewPopupContainer = document.querySelector('.review-popup-container');
const mainContainer = document.querySelector('.main-container');
const showcase = mainContainer.querySelector('.showcase-container');
const showcaseVideo = showcase.querySelector('.showcase-video');
const navigationBar = document.querySelector('.navigation-bar-review');


export function openReviewsPopup() {
  showcaseVideo.style.display = 'none';
  reviewPopupContainer.style.display = 'flex';
  mainContainer.style.filter = 'grayscale(100%) blur(5px)';
  document.body.classList.add('detail-view-open');
}

function closeReviewsPopup() {
  showcaseVideo.style.display = '';

  reviewPopupContainer.style.display = 'none';
  mainContainer.style.filter = 'none';
  document.body.classList.remove('detail-view-open');
}
export function createNavigationBarReviewsPage() {
  navigationBar.innerHTML = `
    <div class="home-button" id="close-review-popup">
          <img src="../public/icons/home.png" alt="home">
    </div>
  `;

  document.getElementById('close-review-popup')?.addEventListener('click', () => {
    closeReviewsPopup();
  });
}
