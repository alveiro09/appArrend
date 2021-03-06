﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;

namespace ArrendApp.Droid
{
    [Activity(Label = "ArrendApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Tema")]
    public class ActMain : ActionBarActivity
    {
        private SupportToolbar mToolbar;
        private DrawerLayout_V7_Tutorial.MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private ActIniciarSesion iniciarSesionFrag;
        private ActCrearUsuario crearUsuarioFrag;        
        private ActCrearInmueble crearInmuebleFrag;
        private ActAdministrarInmueble administrarInmuebleFrag;
        private ActReportes reportesFrag;
        private ActCerrarSesion cerrarSesionFrag;
        private SupportFragment mCurrentFragment = new SupportFragment();
        private Stack<SupportFragment> mStackFragments;

        private ArrayAdapter mLeftAdapter;

        private List<string> mLeftDataSet;
  
 protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.lytMain);

            mToolbar = FindViewById < SupportToolbar > (Resource.Id.toolbar);
            mDrawerLayout = FindViewById < DrawerLayout > (Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById < ListView > (Resource.Id.left_drawer);
            iniciarSesionFrag = new ActIniciarSesion();
            crearUsuarioFrag = new ActCrearUsuario();
            crearInmuebleFrag = new ActCrearInmueble();
            administrarInmuebleFrag = new ActAdministrarInmueble();
            reportesFrag = new ActReportes();
            cerrarSesionFrag = new ActCerrarSesion();
            mStackFragments = new Stack<SupportFragment >();


            mLeftDrawer.Tag = 0;


            SetSupportActionBar(mToolbar);

            mLeftDataSet = new List<string>();
            mLeftDataSet.Add("Iniciar sesión");
            mLeftDataSet.Add("Crear usuario");
            mLeftDataSet.Add("Crear inmueble");
            mLeftDataSet.Add("Administrar inmueble");
            mLeftDataSet.Add("Reportes");
            mLeftDataSet.Add("Cerrar sesión");
            mLeftAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
            mLeftDrawer.Adapter = mLeftAdapter;
            mLeftDrawer.ItemClick += MenuListView_ItemClick;


            mDrawerToggle = new DrawerLayout_V7_Tutorial.MyActionBarDrawerToggle(
            this, //Host Activity
            mDrawerLayout, //DrawerLayout
            Resource.String.DrawerAbierto, //Opened Message
            Resource.String.DrawerCerrado //Closed Message
            );

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            if (bundle != null)
            {
                if (bundle.GetString("DrawerState") == "Opened")
                {
                    SupportActionBar.SetTitle(Resource.String.DrawerAbierto);
                }

                else
                {
                    SupportActionBar.SetTitle(Resource.String.DrawerCerrado);
                }
            }

            else
            {
                //This is the first the time the activity is ran
                SupportActionBar.SetTitle(Resource.String.DrawerCerrado);
            }

            Android.Support.V4.App.FragmentTransaction tx = SupportFragmentManager.BeginTransaction();

            tx.Add(Resource.Id.main, iniciarSesionFrag);
            tx.Add(Resource.Id.main, crearUsuarioFrag);
            tx.Add(Resource.Id.main, crearInmuebleFrag);
            tx.Add(Resource.Id.main, administrarInmuebleFrag);
            tx.Add(Resource.Id.main, reportesFrag);
            tx.Add(Resource.Id.main, cerrarSesionFrag);
            tx.Hide(cerrarSesionFrag);

            mCurrentFragment = iniciarSesionFrag;
            tx.Commit();
        }
        void MenuListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.Support.V4.App.Fragment fragment = null;

            switch (e.Id)
            {
                case 0:
                    ShowFragment(iniciarSesionFrag);
                    break;
                case 1:
                    ShowFragment(crearUsuarioFrag);
                    break;
                case 2:
                    ShowFragment(crearInmuebleFrag);
                    break;
                case 3:
                    ShowFragment(administrarInmuebleFrag);
                    break;
                case 4:
                    ShowFragment(reportesFrag);
                    break;
                case 5:
                    ShowFragment(cerrarSesionFrag);
                    break;
            }

            mDrawerLayout.CloseDrawers();
            mDrawerToggle.SyncState();

            //SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main, fragment).Commit();


            mDrawerLayout.CloseDrawers();
            mDrawerToggle.SyncState();

        }
        private void ShowFragment(SupportFragment fragment)
        {

            if (fragment.IsVisible)
            {
                return;
            }

            var trans = SupportFragmentManager.BeginTransaction();


            fragment.View.BringToFront();
            mCurrentFragment.View.BringToFront();

            trans.Hide(mCurrentFragment);
            trans.Show(fragment);

            trans.AddToBackStack(null);
            mStackFragments.Push(mCurrentFragment);
            trans.Commit();

            mCurrentFragment = fragment;

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;
                    
                case Resource.Id.action_ayuda:
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                outState.PutString("DrawerState", "Opened");
            }

            else
            {
                outState.PutString("DrawerState", "Closed");
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }
    }
}
