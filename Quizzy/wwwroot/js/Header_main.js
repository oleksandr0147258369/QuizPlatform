function toggleMenu(){
    const menu = document.querySelector('.menu-container');
    const burger = document.querySelector('.burger');
    const header = document.querySelector('header');
    const userMenu = document.querySelector('.UserMenu');

    // Track menu state
    const isMenuActive = menu.classList.contains('active');
    const isUserMenuActive = userMenu.classList.contains('active');
    console.log("Menu state:", isMenuActive ? "Active" : "Inactive");
    console.log("User menu state:", isUserMenuActive ? "Active" : "Inactive");

    if(isUserMenuActive){
        userMenu.classList.remove('active');
    }
    
    menu.classList.toggle('active');
    burger.classList.toggle('active');
    header.classList.toggle('active');
    userMenu.classList.remove('active');
}


function toggleIcon(){
    const login = document.querySelector('.Login');
    const userMenu = document.querySelector('.UserMenu');
    login.classList.toggle('active');
    userMenu.classList.toggle('active');

}