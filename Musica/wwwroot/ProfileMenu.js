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
    const button = event.currentTarget;
    const userId = button.getAttribute('user-id');

    switch(action){
        case 'profile':

            window.location.href = '/profile/' + userId;
            break;
        case 'publish':
            window.location.href = '/upload';
            break;
    }
}