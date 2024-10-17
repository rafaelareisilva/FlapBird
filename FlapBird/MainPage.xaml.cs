namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int gravidade=10;
	const int tempoEntreFrames=25;
	bool estaMorto=true;
	double larguraJanela=0;
	double alturaJanela=0;
	int velocidade=20;
	const int maxTempoPulando=3;
	int tempoPulando=0;
	bool estaPulando=false;
	const int forcaPulo=25;
	const int aberturaMinima = 200;
	int score=0;

	
	public MainPage()
	{
		InitializeComponent();
	}

	void AplicaPulo()
	{
      passaro.TranslationY-=forcaPulo;
	  tempoPulando++;
	  if(tempoPulando>=maxTempoPulando)
	  {
		estaPulando=false;
		tempoPulando=0;
	  }
	}

	void AplicaGravidade()
	{
		passaro.TranslationY+=gravidade;
	}
	async Task Desenhar()
	{
		while (!estaMorto)
		{ 
		if (estaPulando)
		     AplicaPulo();

		else
			AplicaGravidade();
			
			GerenciaCanos();

			if (VerificaColisao())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}

		}

	
    

    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
		larguraJanela=w;
		alturaJanela=h;
    }

	void GerenciaCanos()
	{
		CanoDeCima.TranslationX-=velocidade;	
		CanoDeBaixo.TranslationX-=velocidade;
		if(CanoDeBaixo.TranslationX<=-larguraJanela)
		{
			CanoDeBaixo.TranslationX=4;
			CanoDeCima.TranslationX=4;

			var alturaMax = -100;
			var alturaMin = -CanoDeBaixo.HeightRequest;
			CanoDeCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoDeBaixo.TranslationY = CanoDeCima.TranslationY + aberturaMinima + CanoDeBaixo.HeightRequest;

			score++;
			labelScore.Text = "Canos:" + score.ToString("D3");
		}
	
	}

	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		
		Inicializar();
		FrameGameOver.IsVisible=false;
		Desenhar();
	}
	void Inicializar()
	{
		estaMorto=false;
		passaro.TranslationY=0;
	}
	bool VerificaColisao()
	{
		if (!estaMorto)
		{
			if (VerificaColisaoTeto() ||
			VerificaColisaoChao())
			{
				return true;
			}

		}
		return false;
	}
	bool VerificaColisaoTeto()
	{
		var minY = -alturaJanela / 2;
		if (passaro.TranslationY <= minY)
			return true;
		else
			return false;
	}
	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela / 2 ;
		if (passaro.TranslationY >= maxY)
			return true;
		else
			return false;

	}

	void OnGridClicked(object s, TappedEventArgs a)
	{
		estaPulando=true;
	}

}

