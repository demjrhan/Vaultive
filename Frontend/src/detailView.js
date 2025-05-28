import { featuredMovie } from './movieData.js';

const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');
const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const scrollContainer = document.querySelector('.platform-links-detail .logo-images-scroll');

const detailDescription = document.querySelector(
  '.movie-text-description-detail'
);
const showcase = mainContainer.querySelector('.showcase-container');
const moviesPopupContainer = document.querySelector('.movies-popup-container');
const streamingServicePopUpContainer = document.querySelector(
  '.streaming-popup-container'
);
const reviewContent = document.querySelector('.review-content');
const submitReview = document.getElementById('submit-review-button');
const addReview = document.getElementById('add-review-button');
const addReviewContainer = document.getElementById('add-review');
const textarea = document.getElementById('review-textarea');

const API_BASE_URL = 'http://localhost:5034/api';

let detailOpenedFrom = 'home';

export function showMovieDetail(movie, from = 'home') {
  resetTextarea();
  renderPosterAndTrailer(movie);
  renderDetailText(movie);
  renderStreamingLogos(movie);
  openDetailContainer(from);
  renderReviews(movie);
  setupReviewSubmission(movie);
  checkIfReviewButtonIsVisible(movie);
  preventBodyScroll();
}


async function checkIfReviewButtonIsVisible(movie) {
  const mediaId = movie.mediaContent?.id;

  try {
    const response = await fetch(
      `${API_BASE_URL}/WatchHistory/CanUserWriteReview/1/${mediaId}`
    );

    if (!response.ok) {
      toggleAddReviewButton(false);
      return;
    }

    const canWrite = await response.json();

    if (canWrite) {
      toggleAddReviewButton(true);
      setupAddReviewToggle();
    } else {
      toggleAddReviewButton(false);
    }

  } catch (error) {
    console.error('Error checking review permission:', error);
    toggleAddReviewButton(false);
  }
}


function toggleAddReviewButton(show) {
  addReview.classList.toggle('visible', show);
}

function resetTextarea() {
  textarea.value = '';
  textarea.style.color = 'white';
}

function renderPosterAndTrailer(movie) {
  const posterImage = movie.mediaContent?.posterImageName
    ? `../public/img/${movie.mediaContent.posterImageName}.png`
    : '../public/img/default-poster.png';

  detailImage.innerHTML = `
    <iframe
      class='trailer-iframe'
      src='https://www.youtube.com/embed/${movie.mediaContent?.youtubeTrailerURL ?? 'dQw4w9WgXcQ'}?autoplay=1&controls=0&loop=1'
      allow='autoplay; encrypted-media'
      allowfullscreen>
    </iframe>
    <img src='${posterImage}' alt='${movie.mediaContent?.title}'>
  `;
}

function renderDetailText(movie) {
  detailTitle.innerHTML = movie.mediaContent?.title ?? 'Untitled';
  detailDescription.innerHTML = movie.mediaContent?.description ?? 'No description available.';
}

function renderStreamingLogos(movie) {
  const services = movie.mediaContent?.streamingServices || [];
  const count = services.length;

  const logosHTML = services
    .map(s => `
      <a href='${s.websiteLink}' target='_blank'>
        <img src='../public/img/streamers/${s.logoImage}.png' alt='${s.name}'>
      </a>
    `)
    .join('');

  scrollContainer.innerHTML = count > 2 ? logosHTML.repeat(25) : logosHTML;
  scrollContainer.classList.toggle('logo-images-static', count <= 2);
  scrollContainer.classList.toggle('logo-images-scroll', count > 2);
}

function openDetailContainer(from) {
  detailOpenedFrom = from;
  detailContainer.style.display = 'flex';
  mainContainer.style.filter = 'grayscale(100%) blur(10px)';
  showcase.style.backgroundImage = `
    linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
    url(${featuredMovie.backgroundHolder})
  `;

  if (from === 'movies') {
    moviesPopupContainer.style.filter = 'grayscale(100%) blur(10px)';
    moviesPopupContainer.classList.add('overlay-disabled');
  }
  if (from === 'streamingServices') {
    streamingServicePopUpContainer.style.filter = 'grayscale(100%) blur(10px)';
    streamingServicePopUpContainer.classList.add('overlay-disabled');
  }
}
async function renderReviews(movie) {
  reviewContent.innerHTML = '';
  const mediaId = movie.mediaContent?.id;
  console.log(`${API_BASE_URL}/Review/MediaContentsReviews/${mediaId}`);

  try {
    const response = await fetch(
      `${API_BASE_URL}/Review/MediaContentsReviews/${mediaId}`
    );

    if (!response.ok) {
      const p = document.createElement('p');
      p.textContent = 'Something went wrong with fetching reviews. Please try again later.';
      reviewContent.appendChild(p);
      return;
    }

    const reviews = (await response.json()) || [];

    if (reviews.length > 0) {
      reviews.forEach(review => {
        const reviewWrapper = document.createElement('div');
        reviewWrapper.classList.add('review-item');

        const userWrapper = document.createElement('div');
        userWrapper.classList.add('review-user');

        const watchedOn = document.createElement('div');
        watchedOn.classList.add('review-watched-on');
        watchedOn.textContent = review.watchedOn;

        const nickname = document.createElement('div');
        nickname.classList.add('review-nickname');
        nickname.textContent = review.nickname;

        const comment = document.createElement('div');
        comment.classList.add('review-comment');
        comment.textContent = review.comment;

        userWrapper.append(watchedOn, nickname);
        reviewWrapper.append(userWrapper, comment);
        reviewContent.appendChild(reviewWrapper);
      });
    } else {
      const p = document.createElement('p');
      p.textContent = 'No reviews yet. Be the first to write one!';
      reviewContent.appendChild(p);
    }

  } catch (error) {
    const p = document.createElement('p');
    p.textContent = 'Something went wrong with fetching reviews. Please try again later.';
    reviewContent.appendChild(p);
    console.error(error);
  }
}


function setupReviewSubmission(movie) {
  submitReview.addEventListener('mouseover', () => {
    textarea.style.filter = 'blur(2px)';
    textarea.readOnly = true;
  });

  submitReview.addEventListener('mouseout', () => {
    textarea.style.filter = 'blur(0)';
    textarea.readOnly = false;
  });

  submitReview.addEventListener('click', async () => {
    const comment = textarea.value?.trim();
    const mediaId = movie.mediaContent?.id;

    try {
      const response = await fetch(`${API_BASE_URL}/User/Review`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userId: 1, mediaId, comment })
      });

      if (response.ok) {
        setTimeout(() => {
          textarea.style.color = 'green';
          textarea.value =
            'Successfully submitted your review. Thank you!';

          renderReviews(movie);

          detailContainer.scrollTo({
            top: 0,
            behavior: 'smooth'
          });

          toggleAddReviewButton(false);
          addReviewContainer.classList.remove('visible');
          addReviewContainer.style.display = 'none';

          setTimeout(() => {
            textarea.value = '';
            textarea.style.color = 'white';
            textarea.readOnly = false;
          }, 5000);
        }, 1);
      }
    } catch (error) {
      console.error('Review submission failed:', error);
    }
  });
}

function setupAddReviewToggle() {
  addReview.addEventListener('click', () => {
    const isVisible = addReviewContainer.classList.contains('visible');

    if (isVisible) {
      addReview.style.boxShadow = '0 0 35px rgba(255, 255, 255, 1)';
      addReviewContainer.classList.remove('visible');
      setTimeout(() => {
        addReviewContainer.style.display = 'none';
      }, 300);
      addReview.innerText = 'Add';
    } else {
      addReviewContainer.style.display = 'flex';
      addReview.style.boxShadow = '0 0 0 0 rgba(0, 0, 0, 1)';

      requestAnimationFrame(() => {
        addReviewContainer.classList.add('visible');
      });
      textarea.value = '';
      textarea.style.color = 'white';
      textarea.focus();
      addReview.innerText = 'Close';
    }
  });
}

function preventBodyScroll() {
  const scrollY = window.scrollY;
  document.body.style.position = 'fixed';
  document.body.style.top = `-${scrollY}px`;
  document.body.style.width = '100%';
}

export function closeDetailOnEscape() {
  window.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && detailContainer.style.display === 'flex') {

      addReview.style.boxShadow = ' 0 0 35px rgba(255, 255, 255, 1)';
      addReviewContainer.classList.remove('visible');
      setTimeout(() => {
        addReviewContainer.style.display = 'none';
      }, 300);
      addReview.innerText = 'Add';

      textarea.value = '';
      textarea.style.color = 'white';
      textarea.readOnly = false;
      textarea.disabled = false;

      closeDetailView();

      const scrollY = document.body.style.top;
      document.body.style.position = '';
      document.body.style.top = '';
      document.body.style.width = '';
      window.scrollTo(0, parseInt(scrollY || '0') * -1);
      detailContainer.style.display = 'none';
      if (detailOpenedFrom === 'home') {
        showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
        showcase.style.backgroundRepeat = 'no-repeat';
        showcase.style.backgroundSize = 'cover';
        showcase.style.backgroundPosition = 'center';
      }
    }
  });
}

function closeDetailView() {
  detailContainer.scrollTop = 0;
  detailImage.innerHTML = '';
  detailContainer.style.display = 'none';

  if (detailOpenedFrom === 'movies') {
    moviesPopupContainer.style.filter = 'none';
    moviesPopupContainer.classList.remove('overlay-disabled');
  } else if (detailOpenedFrom === 'streamingServices') {
    streamingServicePopUpContainer.style.filter = 'none';
    streamingServicePopUpContainer.classList.remove('overlay-disabled');
  } else {
    mainContainer.style.filter = 'none';
  }

  showcase.classList.remove('overlay-disabled');
}
