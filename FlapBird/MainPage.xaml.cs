﻿namespace FlapBird;

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
			await Task.Delay(tempoEntreFrames);
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
		if(CanoDeBaixo.TranslationX<-larguraJanela)
		{
			CanoDeBaixo.TranslationX=0;
			CanoDeCima.TranslationX=0;
		}
	
	}

	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		FrameGameOver.IsVisible=false;
		Inicializar();
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

