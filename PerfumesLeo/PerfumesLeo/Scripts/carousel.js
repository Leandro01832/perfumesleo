$(document).ready(function(){

	$('.carousel-item:eq(0)').addClass("ativo").show();
	
	var texto = $(".ativo").attr("alt");
	$('.carousel-item').prepend("<p>" + texto + "</p>");

	setInterval(slide, 8000);

	function slide(){
	    if ($('.ativo').next().size()) {
	        $('.ativo').fadeOut().removeClass("ativo").next().addClass("ativo");
	        $('.ativo').delay(500).fadeIn();
		}else
		{
		    $('.ativo').fadeOut().removeClass("ativo");
		    $('.carousel-item:eq(0)').delay(500).fadeIn().addClass("ativo");
		}

		var texto = $(".ativo img").attr("alt");

		$('.carousel-item p').hide().html(texto).delay(1500).fadeIn();

	}

	
     
});