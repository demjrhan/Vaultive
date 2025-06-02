export function renderReviews(reviews) {
  const reviewContent = document.querySelector('.reviews-of-users');

  reviewContent.innerHTML = '';

  const reviewContainer = document.createElement('div');

  reviews.forEach(review => {
    const reviewWrapper = document.createElement('div');
    reviewWrapper.classList.add('review-item');

    const userWrapper = document.createElement('div');
    userWrapper.classList.add('review-user');

    const watchedOn = document.createElement('div');
    watchedOn.classList.add('review-watched-on');
    watchedOn.textContent = review.mediaTitle;

    const nickname = document.createElement('div');
    nickname.classList.add('review-nickname');
    nickname.textContent = review.nickname;

    const comment = document.createElement('div');
    comment.classList.add('review-comment');
    comment.textContent = review.comment;

    userWrapper.append(watchedOn, nickname);
    reviewWrapper.append(userWrapper, comment);
    reviewContainer.append(reviewWrapper);
  });

  reviewContent.appendChild(reviewContainer);
}
