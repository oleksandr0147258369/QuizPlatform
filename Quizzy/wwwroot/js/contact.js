


function toggleMenu(){
    const menu = document.querySelector('.menu-container');
    const burger = document.querySelector('.burger');
    const header = document.querySelector('header');
    const container = document.querySelector('.container');
    menu.classList.toggle('active');
    burger.classList.toggle('active');
    header.classList.toggle('active');
    container.classList.toggle('active');
}


function closeModel(){
    const modelContainer = document.querySelector('.model-container');
    modelContainer.style.display = 'none';
}