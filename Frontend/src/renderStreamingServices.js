import { showMovieDetail } from './detailView.js';

const API_BASE_URL = 'http://localhost:5034/Vaultive';

export async function renderStreamingServices() {
  const popupContentBox = document.querySelector('.streaming-popup-container .content-box-streaming');
  popupContentBox.innerHTML = '';

  try {
    const response = await fetch(`${API_BASE_URL}/GetAllMovies`);
    if (!response.ok) throw new Error('Failed to fetch movies');
    const movies = await response.json();
    const streamingMap = new Map();

    movies.forEach(movie => {
      movie.mediaContent.streamingServices?.forEach(streamingService => {
        if (!streamingMap.has(streamingService.name)) {
          streamingMap.set(streamingService.name, []);
        }
        streamingMap.get(streamingService.name).push(movie);
      });
    });

    const shuffledStreamingServices = [...streamingMap.keys()]
      .sort(() => Math.random() - 0.5); // Simple shuffle

    console.log(shuffledStreamingServices);

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

      streamingMovies.forEach((movie) => {
        const posterImage = movie.mediaContent?.posterImage
          ? `../public/img/${movie.mediaContent.posterImage}.png`
          : '../public/img/default-poster.png';

        const img = document.createElement('img');
        img.src = posterImage;
        img.alt = movie.mediaContent?.title ?? 'Untitled';
        img.addEventListener('click', () => showMovieDetail(movie, 'movies'));
        postersContainer.appendChild(img);
      });


      popupContentBox.appendChild(streamingContainer);
    });

  } catch (error) {
    console.error('Error loading movies:', error);
    popupContentBox.innerHTML =
      '<p class="error-message">Failed to load movies.</p>';
  }
}

