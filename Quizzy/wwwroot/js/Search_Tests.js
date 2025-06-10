
   
function animateValue(id, start, end, duration) {
      const obj = document.getElementById(id);
    const range = end - start;
    const stepTime = Math.abs(Math.floor(duration / range));
    let current = start;

      const timer = setInterval(() => {
        current += 34567;
    obj.textContent = current.toLocaleString('uk-UA');
        if (current >= end) {
        obj.textContent = end.toLocaleString('uk-UA');
    clearInterval(timer);
        }
      }, stepTime);
    }

window.onload = () => {
    animateValue("tests-given", 0, 100, 3000);
    animateValue("tests-completed", 0, 100, 3000);
};
function toggleMenu() {
    const menu = document.querySelector('.menu-container');
    const burger = document.querySelector('.burger');
    const header = document.querySelector('header');
    const container = document.querySelector('.container');
    menu.classList.toggle('active');
    burger.classList.toggle('active');
    header.classList.toggle('active');
    container.classList.toggle('active');
}
