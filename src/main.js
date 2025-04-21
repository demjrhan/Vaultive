const detailContainer = document.querySelector('.main-container-detail');
const mainContainer = document.querySelector('.main-container');

const featuredMovie = {
  publisher: 'A Disney Original Film',
  title: 'RAYA AND THE LAST DRAGON',
  imageSrc: 'public/img/raya-the-last-dragon-star.png',
  backgroundGif: '../public/gif/raya.gif',
  backgroundHolder: '../public/img/raya-the-last-dragon-backgroundHolder.jpeg',
};

const recommendations = [
  { id: 'raya', src: './public/img/raya-the-last-dragon.jpeg', alt: 'Raya' },
  { id: 'johnwick', src: './public/img/john_wick.png', alt: 'John Wick' },
  { id: 'deadpool', src: './public/img/deadpool.png', alt: 'Deadpool' },
  { id: 'westworld', src: './public/img/westworld.png', alt: 'Westworld' },
  { id: 'avangers', src: './public/img/avangers.png', alt: 'Avangers' },
];

const details = [
  {
    id: 'raya',
    src: 'public/img/raya-the-last-dragon.jpeg',
    alt: 'Raya',
    title: 'Raya',
    description:
      'A fearless warrior named Raya embarks on a quest to find the last dragon...',
    background: "url('../public/gif/raya.gif')",
  },
  {
    id: 'johnwick',
    src: 'public/img/john_wick.png',
    alt: 'John Wick',
    title: 'John Wick',
    description:
      'A retired hitman seeks vengeance against those who wronged him...',
    background: "url('../public/gif/john-wick.gif')",
  },
  {
    id: 'deadpool',
    src: 'public/img/deadpool.png',
    alt: 'Deadpool',
    title: 'Deadpool',
    description:
      'Wade Wilson becomes the anti-hero Deadpool after a rogue experiment...',
    background: "url('../public/gif/deadpool.gif')",
  },
  {
    id: 'westworld',
    src: 'public/img/westworld.png',
    alt: 'Westworld',
    title: 'Westworld',
    description:
      'A futuristic theme park where AI hosts begin to gain consciousness...',
    background: "url('./gif/raya.gif')",
  },
  {
    id: 'avangers',
    src: 'public/img/avangers.png',
    alt: 'Avangers',
    title: 'Avengers',
    description:
      "Earth's mightiest heroes unite to fight threats beyond any one hero's capabilities...",
    background: "url('./gif/raya.gif')",
  },
];

const starContainer = document.querySelector('.star-movies-container');
starContainer.innerHTML = `
  <h3 class="star-movie-publisher">${featuredMovie.publisher}</h3>
  <h1>${featuredMovie.title}</h1>
  <img class="star-movie-img" src="${featuredMovie.imageSrc}" alt="${featuredMovie.title}">
`;

const movieCardsContainer = document.querySelector('.movie-cards');
movieCardsContainer.innerHTML = '';

const detailImage = document.querySelector('.movie-image-detail');
const detailTitle = document.querySelector('.movie-title-detail');
const detailDescription = document.querySelector(
  '.movie-text-description-detail',
);
const showcase = mainContainer.querySelector('.showcase-container');

recommendations.forEach((movie) => {
  const img = document.createElement('img');
  img.className = 'movie-cards-img';
  img.src = movie.src;
  img.alt = movie.alt;

  img.addEventListener('click', () => {
    const detail = details.find((d) => d.id === movie.id) || {
      src: details.src,
      alt: details.alt,
      title: details.alt,
      description: details.description,
      background: details.background,
    };
    detailImage.innerHTML = `<img src=${movie.src} alt=${movie.alt}>`;
    detailTitle.innerHTML = detail.title;
    detailDescription.innerHTML = detail.description;

    detailImage.style.backgroundImage = `
    linear-gradient(to bottom, rgba(0,0,0, 0) 0%, rgba(0,0,0, 1) 100%),
    ${detail.background}
  `;

    detailContainer.style.display = 'flex';
    mainContainer.style.filter = 'grayscale(100%) blur(5px)';
    showcase.style.backgroundImage = `linear-gradient(to bottom, rgba(0, 0, 0, 0.55) 0%, rgba(0, 0, 0, 1) 100%),
                                      url(${featuredMovie.backgroundHolder})`;
  });

  movieCardsContainer.appendChild(img);
});

window.onclick = function (event) {
  if (event.target === detailContainer) {
    detailContainer.style.display = 'none';
    mainContainer.style.filter = 'grayscale(0%) blur(0px)';
  }
};

window.addEventListener('keydown', (e) => {
  if (e.key === 'Escape') {
    detailContainer.style.display = 'none';
    mainContainer.style.filter = 'grayscale(0%) blur(0px)';
    showcase.style.backgroundImage = `url(${featuredMovie.backgroundGif})`;
    showcase.style.backgroundRepeat = 'no-repeat';
    showcase.style.backgroundSize = 'cover';
    showcase.style.backgroundPosition = 'center';
  }
});
