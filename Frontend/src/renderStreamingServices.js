import { showMovieDetail } from './detailView.js';


export async function renderStreamingServices(movies) {
  const popupContentBox = document.querySelector('.streaming-popup-container .content-box-streaming');


  popupContentBox.innerHTML = '';

    const streamingMap = new Map();

    movies.forEach(movie => {
      movie.mediaContent.streamingServices?.forEach(streamingService => {
        if (!streamingMap.has(streamingService.name)) {
          streamingMap.set(streamingService.name, []);
        }
        streamingMap.get(streamingService.name).push(movie);
      });
    });

     /* Used spread operator ... in order to get only the streaming service names.*/
    const shuffledStreamingServices = [...streamingMap.keys()]
      .sort(() => Math.random() - 0.5);

    /* Now the array more or less looks like this ["HBO" ["Movie", "Movie"]]
    * I created container for each streaming services and adding all movie posters as image. And all
    * images has eventListener for click. Clicking will bring details. */
    shuffledStreamingServices.forEach(streamingServiceName => {
      const streamingMovies = streamingMap.get(streamingServiceName);
      const streamingContainer = document.createElement('div');
      streamingContainer.className = 'streaming-container';

      streamingContainer.innerHTML = `
        <div class="title-buttons-wrapper">
          <h2 class="genre-title">${streamingServiceName}</h2>
          <div class="scroll-buttons">
            <button class="scroll-button left">&#10094;</button>
            <button class="scroll-button right">&#10095;</button>
          </div>
        </div>
        <div class="movies">
          <div class="movie-posters"></div>
        </div>
      `;


      const postersContainer = streamingContainer.querySelector('.movie-posters');
      const leftButton = streamingContainer.querySelector('.scroll-button.left');
      const rightButton = streamingContainer.querySelector('.scroll-button.right');

      leftButton.addEventListener('click', () => {
        postersContainer.scrollBy({ left: -300, behavior: 'smooth' });
      });

      rightButton.addEventListener('click', () => {
        postersContainer.scrollBy({ left: 300, behavior: 'smooth' });
      });


      streamingMovies.forEach((movie) => {
        const posterImage = movie.mediaContent?.posterImageName
          ? `../public/img/${movie.mediaContent.posterImageName}.png`
          : '../public/img/default-poster.png';

        const img = document.createElement('img');
        img.src = posterImage;
        img.alt = movie.mediaContent?.title ?? 'Untitled';
        img.addEventListener('click', () => showMovieDetail(movie, 'streamingServices'));
        postersContainer.appendChild(img);
      });


      popupContentBox.appendChild(streamingContainer);
    });


}

