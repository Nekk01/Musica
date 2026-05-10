const dropdownMenu = document.getElementById('dropdownMenu');
const profileIcon = document.getElementById('profileIcon');
let isMenuOpen = false;

function toggleMenu(){
    isMenuOpen = !isMenuOpen;
    if(isMenuOpen){
        dropdownMenu.classList.add('show');
    }
    else{
        dropdownMenu.classList.remove('show');
    }
}

document.addEventListener('click', function(e) {
    if (!profileIcon.contains(e.target)) {
        dropdownMenu.classList.remove('show');
        isMenuOpen = false;
    }
});

function handleAction(action){
    switch(action){
        case 'profile':
            window.location.href = '/profile';
            break;
    }
}