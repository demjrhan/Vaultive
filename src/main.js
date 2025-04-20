  const movieCards = document.querySelectorAll(".movie-cards-img");
  const detailContainer = document.querySelector(".main-container-detail");
  const mainContainer = document.querySelector(".main-container");

  movieCards.forEach(card => {
  card.addEventListener("click", () => {
    detailContainer.style.display = "flex";
    mainContainer.style.filter = "grayscale(100%) blur(5px)";
  });
});

  window.onclick = function(event) {
  if (event.target === detailContainer) {
  detailContainer.style.display = "none";
}
};
  window.addEventListener('keydown', (e) => {
    if (e.key === "Escape") {
      detailContainer.style.display = "none";
      mainContainer.style.filter = "grayscale(0%) blur(0px)";
    }
  });