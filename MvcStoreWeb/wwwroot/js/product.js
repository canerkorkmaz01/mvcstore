$(() => {
    $('#decrease').click(decrease);

    $('#increase').click(increase);

    $('#quantity').keydown(function (e) {
        var isNumber = /^\d+$/.test(e.originalEvent.key);

        if (e.originalEvent.keyCode != 8 && !isNumber) {
            e.preventDefault();
            return;
        }
    });

    $('#quantity').change(function (e) {
        const v = $(e.currentTarget).val();
        if (v == null || v == '') {
            $(e.currentTarget).val(1);
        }
    });

    function decrease(e) {
        var v = $('#quantity').val();
        if (v > 1) {
            v--;
            $('#quantity').val(v);
        }

    }

    function increase(e) {
        var v = $('#quantity').val();
        v++;
        $('#quantity').val(v);
    }

    $('.thumb-image').click((e) => {
        $('.thumb-image').removeClass('active');
        $(e.currentTarget).addClass('active');
        $('#product-image').attr('src', $(e.currentTarget).attr('src'));
    });

    $('.rate-star').click((e) => {
        const star = e.currentTarget;
        const val = $(star).attr('data-val');
        $('.rate-star').removeClass('fas').addClass('far');
        for (var i = 1; i <= val; i++) {
            $('.rate-star[data-val=' + i + ']').removeClass('far').addClass('fas');
        }
        $('#Rate').val(val);
    });

    $('#commentForm').submit((e) => {
        const val = $('#Rate').val();
        if (val == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                html: 'Lütfen bir puan veriniz.'
            });
            return false;
        }
    });

    $('.like-btn').click((e) => {
        like(true, e.currentTarget);
    });

    $('.dislike-btn').click((e) => {
        like(false, e.currentTarget);
    });

    like = (l, o) => {
        const commentId = $(o).attr('data-id');
        const userId = $(o).attr('data-userId');
        $.getJSON(`https://localhost:5001/Home/likecomment/${commentId}?like=${l}&userId=${userId}`)
            .then((response) => {
                if (response.error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata',
                        html: `Daha önce ${l ? "beğendiniz" : "beğenmediniz"}`
                    });
                } else {
                    $(o).find('.count').html(l ? response.likes : response.dislikes);
                }
            });
    };

});