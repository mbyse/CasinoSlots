using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mega_Challenge_Casino
{
    public partial class Default : System.Web.UI.Page
    {      
        string[] Images = { "Bar.png", "Bell.png", "Cherry.png", "Clover.png", "Diamond.png", "HorseShoe.png", "Lemon.png", "Orange.png", "Plum.png", "Seven.png", "Strawberry.png", "Watermellon.png" };

        int playersMoney { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {

            //Display player money                       
            if (ViewState["playersMoney"] == null)
            {
                playersMoney = 100;
                playerMoneyLabel.Text = "$100";

            }
            else
            {
                playersMoney = int.Parse(ViewState["playersMoney"].ToString());
                playerMoneyLabel.Text = string.Format("${0}", playersMoney);

            }

        }

        public void leverButton_Click(object sender, EventArgs e)
        {
            //Define bet
            int bet = int.Parse(betTextBox.Text);
            int betReturn = 0;

            if (bet > playersMoney)
            {
                resultLabel2.Text = "You can't bet more money than you have!!";
            }

            else
            {
                //Randomly Load Images

                //Generate three random numbers and put into list
                List<int> imageNo = new List<int>();
                int cherryNo = 0;
                int sevenNo = 0;
                int barNo = 0;
                int otherNo = 0;

                Random random = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int imageID = random.Next(0, 12);
                    imageNo.Add(imageID);
                    if (imageID == 2)
                    {
                        cherryNo++;
                    }
                    else if (imageID == 9)
                    {
                        sevenNo++;
                    }
                    else if (imageID == 0)
                    {
                        barNo++;
                    }
                    else
                    {
                        otherNo++;
                    }
                }

                //Correlate each number in list with Images[]
                Image1.ImageUrl = string.Format("~/Images/{0}", Images[imageNo[0]]);

                Image2.ImageUrl = string.Format("~/Images/{0}", Images[imageNo[1]]);

                Image3.ImageUrl = string.Format("~/Images/{0}", Images[imageNo[2]]);

                //Determine and display bet result
                if (barNo >= 1)
                {
                    int result = bet;
                    resultLabel.Text = string.Format("Sorry, you lost ${0}. Better luck next time.", result);
                    betReturn = -result;
                }
                else if (sevenNo == 3)
                {
                    int result = bet * 100;
                    resultLabel.Text = string.Format("You bet ${0} and won ${1}!", bet, result);
                    betReturn = result;
                }
                else if (cherryNo >= 1)
                {
                    int result = bet * (cherryNo + 1);
                    resultLabel.Text = string.Format("You bet ${0} and won ${1}!", bet, result);
                    betReturn = result;
                }
                else
                {
                    int result = bet;
                    resultLabel.Text = string.Format("Sorry, you lost ${0}. Better luck next time.", result);
                    betReturn = -result;
                }

                //Display Player's New Total

                if (ViewState["playersMoney"] == null)
                {
                    int playersMoney = 100;
                    int total = playersMoney + betReturn;
                    playerMoneyLabel.Text = string.Format("${0}", total);
                    ViewState.Add("playersMoney", total);
                }
                else
                {
                    int playersMoney = int.Parse(ViewState["playersMoney"].ToString());
                    int total = playersMoney + betReturn;

                    if (total <= 0)
                    {
                        playerMoneyLabel.Text = string.Format("${0}", total);
                        ViewState.Add("playersMoney", total);
                        resultLabel2.Text = "Sorry, you are out of money! Refresh the page to try your luck again.";
                    }
                    else
                    {
                        playerMoneyLabel.Text = string.Format("${0}", total);
                        ViewState.Add("playersMoney", total);
                    }

                }

                //Clear List
                imageNo.Clear();
            }
        }
    }
}

/* Instructor Solution:
* public partial class Default : System.Web.UI.Page
* {           
*   Random random = new Random();
* }
* 
* protected void Page_Load(object sender, EventArgs e)
* {
*   if (!Page.IsPostBack)
*   {
*       string[] reels = new string[] { spinReel(), spinReel(), spinReel()};
*       displayImages(reels);
*       ViewState.Add("PlayersMoney", 100);
*       displayPlayersMoney();
*    }
* }
* 
* private void displayPlayersMoney()
* {
*   moneyLabel.Text = String.Format ("Player's Money: {0:C}", ViewState["PlayersMoney"]);
* }
* 
* protected void leverButton_Click(object sender, EventArgs e)
* {
*   int bet = 0;
*   if (!int.TryParse(betTextBox.Text, out bet)) return;
*   
*   int winnings = pullLever(bet);
*   displayResult(bet, winnings);
*   adjustPlayersMoney(bet, winnings);
*   displayPlayersMoney();
* }
* 
* private void adjustPlayersMoney(int bet, int winnings)
* {
*   int playersMoney = int.Parse(ViewState["PlayersMoney"].ToString());
*   playersMoney -= bet;
*   playersMoney += winnings;
*   ViewState["PlayersMoney"] = playersMoney
* }
* 
* private void displayResult(int bet, int winnings)
* {
*   if (winnings > 0)
*   {
*       resultLabel.Text = String.Format("You bet {0:C} and won {1:C}!", bet, winnings)
*   }
*   else
*   {
*       resultLabel.Text = String.Format("Sorry you lost {0:C}. Better luck next time!", bet}
*   }
* }
* 
* private int pullLever(int bet)
* {
*      string[] reels = new string[]{spinReel(), spinReel(), spinReel() };
*      displayImages(reels);
*      
*      int multiplier = evaluateSpin(reels);
*      return bet * multiplier;
* }
* 
* private int evaluateSpin(string[] reels)
* {
*   //If one bar, return 0;
*   if (isBar(reels)) return 0;
*   //If three 7's, then return 100;
*   if (isJackpot(reels)) return 100;
*   //If there's one or more cherries, return 2,3,4;
*   int multiplier = 0;
*   if (isWinner(reels, out multiplier)) return multiplier;
*   return 0;
* }
* 
* private bool isBar(string[] reels)
* {
*   if (reels[0] == "Bar" || reels[1] == "Bar" || reels[2] == "Bar")
*       return true;
*   else
*       return false;
* }
* 
* private bool isJackpot(string[] reels)
* {
*   if(reels[0] == "Seven" && reels[1] == "Seven" && reels[2] == "Seven")
*       return true;
*   else
*       return false;
* }
* 
* private bool isWinner(string[] reels, out int multiplier)
* {
*   multiplier = determineMultiplier(reels);
*   if (multiplier >0) return true;
*   return false;
* }  
*   
* private int determineMultiplier(string[] reels)
* {   
*   int cherryCount = determineCherryCount(reels);
*   if (cherryCount ==1) return 2;
*   if (cherryCount ==2) return 3;
*   if (cherryCount ==3) return 4;
*   return 0;
*  }
*   
*  private int determineCherryCount(string[] reels)
*  {
*   int cherryCount = 0;
*   if (reels[0] == "Cherry") cherryCount++;
*   if (reels[1] == "Cherry") cherryCount++;
*   if (reels[2] == "Cherry") cherryCount++;
*   return cherryCount;
*   }
*   
* private void displayImages(string[] reels)
* {
*  Image1.ImageUrl = "/Images/" + reels[0] + ".png";
*  Image2.ImageUrl  ="/Images/" + reels[1] + ".png";
*  Image3.ImageUrl = "/Images/" + reels[2] + ".png";
* 
* private string spinReel()
* {string [] images = new string[] {"Bar", "Bell", etc.....}
*  return images[random.Next(11)];
* }
* 
* 
*/