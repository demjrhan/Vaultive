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
  { id: 'godfather', src: './public/img/godfather.jpg', alt: 'God Father' },
  {
    id: 'pulpfiction',
    src: './public/img/pulpfiction.jpg',
    alt: 'Pulp Fiction',
  },
  { id: 'johnwick', src: './public/img/john_wick.png', alt: 'John Wick' },
  { id: 'deadpool', src: './public/img/deadpool.png', alt: 'Deadpool' },
  { id: 'avengers', src: './public/img/avengers.png', alt: 'Avengers' },
  { id: 'raya', src: './public/img/raya-the-last-dragon.jpeg', alt: 'Raya' },
];

const details = [
  {
    id: 'raya',
    src: 'public/img/raya-the-last-dragon.jpeg',
    alt: 'Raya',
    title: 'Raya',
    description:
      'Raya is a skilled warrior on a quest to find the last dragon and unite the fractured land of Kumandra. The movie blends Southeast Asian culture with stunning animation and heartfelt storytelling. As she faces challenges and betrayals, Raya learns the true meaning of trust and unity. This fantasy adventure is rich in emotion, action, and mythical wonder.',
    background: "url('../public/gif/raya.gif')",
  },
  {
    id: 'johnwick',
    src: 'public/img/john_wick.png',
    alt: 'John Wick',
    title: 'John Wick',
    description: "John Wick is a former assassin drawn back into the criminal underworld after the death of his beloved dog, a final gift from his late wife. The film is known for its stylish action sequences, intense choreography, and iconic gun-fu combat. Keanu Reeves delivers a powerful performance as a man driven by grief and vengeance. It sets a new standard for modern action films with a unique visual style.",
    background: "url('../public/gif/john-wick.gif')",
  },
  {
    id: 'deadpool',
    src: 'public/img/deadpool.png',
    alt: 'Deadpool',
    title: 'Deadpool',
    description: "Deadpool tells the story of Wade Wilson, a former special forces operative who becomes a wise-cracking anti-hero after a rogue experiment. Breaking the fourth wall, Deadpool delivers nonstop humor and over-the-top violence. The film is a refreshing and irreverent take on the superhero genre. It balances its chaotic energy with a surprisingly emotional backstory.",
    background: "url('../public/gif/deadpool.gif')",
  },
  {
    id: 'pulpfiction',
    src: 'public/img/pulpfiction.png',
    alt: 'Pulp Fiction',
    title: 'Pulp Fiction',
    description: "Pulp Fiction is a cult classic directed by Quentin Tarantino, weaving multiple crime stories in a non-linear narrative. It features unforgettable characters like Vincent Vega and Jules Winnfield, whose dialogue has become iconic. The film is praised for its unique storytelling, eclectic soundtrack, and dark humor. It’s a landmark in independent cinema and a must-watch for film lovers.",
    background: "url('../public/gif/pulpfiction.gif')",
  },
  {
    id: 'avengers',
    src: 'public/img/avengers.png',
    alt: 'Avengers',
    title: 'Avengers',
    description: "Avengers brings together Marvel's greatest superheroes in an epic battle to save the world from Loki and the Chitauri army. It’s a thrilling culmination of the MCU's Phase One, filled with action, humor, and memorable team dynamics. The chemistry between Iron Man, Captain America, Thor, Hulk, Black Widow, and Hawkeye is electric. This blockbuster redefined superhero ensemble films.",
    background: "url('../public/gif/avengers.gif')",
  },
  {
    id: 'godfather',
    src: 'public/img/godfather.jpg',
    alt: 'God Father',
    title: 'God Father',
    description: "The Godfather is a cinematic masterpiece that chronicles the rise of Michael Corleone in the powerful Italian-American mafia family. It explores themes of loyalty, power, and family through unforgettable storytelling. The film’s direction, performances, and score are critically acclaimed and deeply influential. It remains one of the greatest and most respected films in history.",
    background: "url('../public/gif/godfather.gif')",
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
