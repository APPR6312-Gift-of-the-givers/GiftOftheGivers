// Lets a donor click a preset amount button to fill the Amount field.
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.preset-amount').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var amountInput = document.getElementById('Amount');
            if (amountInput) {
                amountInput.value = btn.dataset.amount;
            }
            document.querySelectorAll('.preset-amount').forEach(function (b) { b.classList.remove('btn-green'); b.classList.add('btn-outline-amount'); });
            btn.classList.remove('btn-outline-amount');
            btn.classList.add('btn-green');
        });
    });

    document.querySelectorAll('.auth-tab-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var target = btn.dataset.target;
            document.querySelectorAll('.auth-tab-btn').forEach(function (b) { b.classList.remove('active'); });
            btn.classList.add('active');
            document.querySelectorAll('.auth-panel').forEach(function (p) { p.style.display = 'none'; });
            var panel = document.getElementById(target);
            if (panel) panel.style.display = 'block';
        });
    });
});
