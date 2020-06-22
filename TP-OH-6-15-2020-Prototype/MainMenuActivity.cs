﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TP_OH_6_15_2020_Prototype.Models;
using ZXing.Mobile;

namespace TP_OH_6_15_2020_Prototype
{
    [Activity(Label = "MainMenuActivity")]
    public class MainMenuActivity : Activity
    {
        private TextView welcomeTextView;
        private Button viewEventBtn;
        private Button takeQuizesBtn;
        private Button redeemAwardsBtn;
        private Button findOutAboutIITBtn;
        private Button scanQRCodeBtn;
        private UserModel nameResponse;

        public static int UserId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.main_menu_layout);
            InitView();
        }

        private async void InitView()
        {
            welcomeTextView = FindViewById<TextView>(Resource.Id.welcomeTextView);
            viewEventBtn = FindViewById<Button>(Resource.Id.viewEventsBtn);
            takeQuizesBtn = FindViewById<Button>(Resource.Id.takeQuizesBtn);
            redeemAwardsBtn = FindViewById<Button>(Resource.Id.redeemAwardsBtn);
            findOutAboutIITBtn = FindViewById<Button>(Resource.Id.findOutAboutIITBtn);
            scanQRCodeBtn = FindViewById<Button>(Resource.Id.scanQRCodeBtn);
            var userid = Intent.GetIntExtra("userid", -1);
            UserId = userid;
            var nameRequest = await WebRequest.HttpClient.GetAsync($"http://10.0.2.2:54888/Users/GetUserInfo?userid={userid}");

            nameResponse = JsonConvert.DeserializeObject<UserModel>(await nameRequest.Content.ReadAsStringAsync());

            welcomeTextView.Text = $"Welcome to TP, {nameResponse.username}!";

            viewEventBtn.Click += ViewEventBtn_Click;
            takeQuizesBtn.Click += TakeQuizesBtn_Click;
            scanQRCodeBtn.Click += ScanQRCodeBtn_Click;
        }

        private async void ScanQRCodeBtn_Click(object sender, EventArgs e)
        {
            MobileBarcodeScanner.Initialize(Application);
            var scanner = new MobileBarcodeScanner();

            var result = await scanner.Scan();

            if (result != null)
            {
                Console.WriteLine("Scanned Barcode: " + result.Text);
                var rewardClaimRequest = await WebRequest
                    .HttpClient.GetAsync($"http://10.0.2.2:54888/EventsTables/ClaimRewardFromEvent?userId={UserId}&rewardCode={result.Text}");
                if (rewardClaimRequest.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Toast.MakeText(this, "Sorry, you have already claimed this award", ToastLength.Short).Show();
                }
                else if (rewardClaimRequest.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Toast.MakeText(this, "Sorry, this is an invalid code", ToastLength.Short).Show();
                }
                else if (rewardClaimRequest.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    Toast.MakeText(this, "An error occurred, please try again", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Reward claimed, thank you for participating!", ToastLength.Short).Show();
                }
            }
        }

        private void TakeQuizesBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QuizListActivity));
            StartActivity(intent);
        }

        private void ViewEventBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(EventListActivity));
            StartActivity(intent);
        }
    }
}