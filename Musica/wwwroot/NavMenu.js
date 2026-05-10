function setActiveBasedOnUrl() {
    let currentPage = window.location.pathname;
    
    currentPage = currentPage.replace(/^\//, '');
    
    if (currentPage === '') {
        currentPage = 'home';
    }

    document.querySelectorAll('.button').forEach(button => {
        const buttonHref = button.getAttribute('href');
        
        if (buttonHref === currentPage) {
            button.classList.add('active');
        } else {
            button.classList.remove('active');
        }
    });
}

document.querySelectorAll('.button').forEach(button => {
    button.addEventListener('click', function(e) {
        e.preventDefault();
        
        const page = this.getAttribute('href');
        
        document.querySelectorAll('.button').forEach(btn => {
            btn.classList.remove('active');
        });
        this.classList.add('active');
        
        if (typeof handleAction === 'function') {
            handleAction(page);
        }
        window.location.href = page;
    });
});

window.addEventListener('DOMContentLoaded', setActiveBasedOnUrl);

function handleAction(page) {
    console.log('Action:', page);
}