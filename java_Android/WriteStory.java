package com.RiddleBrothers.Poe.WriteActivities;

import android.app.ActionBar;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Resources;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.Html;
import android.util.Log;
import android.view.View;
import android.widget.*;
import com.RiddleBrothers.Poe.MyActivity;
import com.RiddleBrothers.Poe.R;
import com.RiddleBrothers.Poe.Settings.MyApp;
import com.RiddleBrothers.Poe.json.JsonPost;
import com.RiddleBrothers.Poe.json.LoginCheck;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import java.util.Timer;
import java.util.TimerTask;


/**
 * Created with IntelliJ IDEA.
 * User: Mark Kamberger
 * Date: 11/28/12
 * Time: 2:46 PM
 * To change this template use File | Settings | File Templates.
 */
public class WriteStory extends Activity {

    private boolean b_isBold;
    private boolean b_isUl;
    private boolean b_isItalic;
    private boolean b_isVisible;
    private LinearLayout ll_toolLayout;
    private ImageButton ib_toolShowHide;
    private TextEditor ed_storyContent;
    private EditText ed_title;
    private EditText ed_subTitle;
    private Spinner sp_genre;
    private Button bt_draft;
    private Button bt_publish;
    private String _storyID;
    private ProgressDialog progressDialog;
    private Timer _autoSaveTimer;
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle bundle = this.getIntent().getExtras();
        _storyID = bundle.getString("storyID");

        setContentView(R.layout.write_story);
        Initialize();
        if(_storyID.equals("new")){
        new CheckForAutoSave().execute();}
        else {
            new EditDraft(_storyID).execute();

        }
        ActionBar actionBar = getActionBar();
        actionBar.hide();
    }

     private void Initialize()
     {
         //EditText
         ed_storyContent = (TextEditor)findViewById(R.id.write_story_editText);
         ed_subTitle = (EditText)findViewById(R.id.write_subTitle);
         ed_title = (EditText)findViewById(R.id.write_title);
         //Linear
         ll_toolLayout = (LinearLayout)findViewById(R.id.write_toolsLayout);
         ll_toolLayout.setVisibility(View.GONE);
         //ImageButton
         ib_toolShowHide = (ImageButton)findViewById(R.id.write_btnTools);
         ib_toolShowHide.setOnClickListener(new View.OnClickListener() {
             @Override
             public void onClick(View view) {
                 Resources res = getApplicationContext().getResources();
                 if(b_isVisible){
                   ll_toolLayout.setVisibility(View.GONE);
                   ib_toolShowHide.setImageDrawable(res.getDrawable(R.drawable.br_down_icon));
                 }
                 else {
                     ll_toolLayout.setVisibility(View.VISIBLE);
                     ib_toolShowHide.setImageDrawable(res.getDrawable(R.drawable.br_up_icon));
                 }
                 b_isVisible = !b_isVisible;
             }
         });
         //Buttons
         bt_draft = (Button)findViewById(R.id.write_btnDraft);
         bt_draft.setOnClickListener(new View.OnClickListener() {
             @Override
             public void onClick(View view) {
                 new SubmitStory("Draft").execute();
             }
         });
         bt_publish = (Button)findViewById(R.id.write_btnPublish);
         bt_publish.setOnClickListener(new View.OnClickListener() {
             @Override
             public void onClick(View view) {
                new SubmitStory("Published").execute();
             }
         });
         //Spinner
         sp_genre = (Spinner)findViewById(R.id.write_genre);
         //ProgressDialog
         progressDialog = new ProgressDialog(this);
         progressDialog.setMessage("Fetching...");
         progressDialog.setIndeterminate(true);
         progressDialog.setCancelable(false);

     }
    @Override
    public void onResume() {
        super.onResume();
        _autoSaveTimer = new Timer();
        _autoSaveTimer.schedule(new TimerTask() {
            @Override
            public void run() {
                runOnUiThread(new Runnable() {
                    public void run() {
                          new AutoSaveDraft().execute();

                    }
                });
            }
        }, 0, 10000); // updates each 10 secs
    }
    @Override
    public void onPause() {
        _autoSaveTimer.cancel();
        super.onPause();
    }
    @Override
    public void onRestart() {  // After a pause OR at startup
        super.onResume();
        new LoginCheck() {
            @Override
            public void LoggedIn() {
                //To change body of implemented methods use File | Settings | File Templates.
            }

            @Override
            public void LoggedOut() {
                Intent intent = new Intent(WriteStory.this,MyActivity.class);
                startActivity(intent);
            }
        };

    }

    private class SubmitStory
            extends AsyncTask {
        private String publishStatus;
        private boolean success;
        private String message;

        public SubmitStory(String publishSatus){
            progressDialog.show();
                                     this.publishStatus = publishSatus;
        }

        @Override
        protected Object doInBackground(Object... objects) {
            try {

                JSONStringer jsonSend = new JSONStringer()
                        .object()
                        .key("StoryContent").value(ed_storyContent.getTextHTML())

                        .key("Title").value(ed_title.getText().toString())
                        .key("Synopsis").value(ed_subTitle.getText().toString())
                        .key("Genre").value(sp_genre.getSelectedItem())
                        .key("PublishStatus").value(publishStatus)
                        .key("StoryID").value(_storyID)
                        .endObject();
                JSONObject json = JsonPost.postJSONtoURL(MyApp.getServiceUrl() + "/Api/Story/SaveStory", jsonSend);
                if(json == null)
                {
                    success = false;
                    message = "No response";
                }
                else {
                    try{
                        success =  json.getBoolean("Success");
                        message = json.getString("Message");
                    }
                    catch (Exception e){
                        success = false;
                        message = e.getMessage();
                    }

                }
                WriteStory.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                           progressDialog.cancel();
                        if(!success){
                            Toast.makeText(WriteStory.this,message,Toast.LENGTH_LONG).show();
                        }
                        else {
                            Toast.makeText(WriteStory.this,"Story Saved",Toast.LENGTH_LONG).show();
                            finish();
                        }

                    }
                });

            } catch (JSONException e) {
                Log.e("log_tag", "Error parsing data " + e.toString());
            }

            return null;
        }
    }
    private class CheckForAutoSave
            extends AsyncTask {
        private String publishStatus;
        private boolean success;
        private String message;
        private String storyContent;
        private String title;
        private String synopsis;

        public CheckForAutoSave( ){
            progressDialog.show();
        }

        @Override
        protected Object doInBackground(Object... objects) {
            try {
                JSONStringer jsonSend = new JSONStringer()
                        .object()
                        .endObject();
                JSONObject json = JsonPost.postJSONtoURL(MyApp.getServiceUrl() + "/Api/Story/CheckAutoSave", jsonSend);
                if(json == null)
                {
                    success = false;
                    message = "No response";
                }
                else {
                    try{
                        success =  json.getBoolean("Success");
                        message = json.getString("Message");
                        storyContent = json.getString("StoryContent");
                        title = json.getString("Title");
                        synopsis = json.getString("Synopsis");
                    }
                    catch (JSONException e){
                        success = false;
                        message = e.getMessage();
                    }

                }
                WriteStory.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        progressDialog.cancel();
                        if(success){
                            AlertDialog ad = new AlertDialog.Builder(WriteStory.this).create();
                            ad.setTitle("Unsaved Story");
                            ad.setMessage("There is an unsaved story you were working on, would you like to edit or ignore the story?");
                            ad.setButton("Edit", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialogInterface, int i) {
                                    ed_storyContent.setText(Html.fromHtml(storyContent));
                                    ed_subTitle.setText(synopsis);
                                    ed_title.setText(title);
                                }

                            });

                            ad.setButton2("Ignore", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialogInterface, int i) {
                                    //To change body of implemented methods use File | Settings | File Templates.
                                }
                            });
                            ad.show();
                        }
                        else {

                        }

                    }
                });

            } catch (Exception e) {
                Log.e("log_tag", "Error parsing data " + e.toString());
            }

            return null;
        }
    }

    private class EditDraft
            extends AsyncTask {
        private String publishStatus;
        private boolean success;
        private String message;
        private String storyContent;
        private String title;
        private String synopsis;
        private String id;

        public EditDraft(String storyID ){
            progressDialog.show();
            id= storyID;
        }

        @Override
        protected Object doInBackground(Object... objects) {
            try {
                JSONStringer jsonSend = new JSONStringer()
                        .object()
                        .key("Id").value(id)
                        .endObject();
                JSONObject json = JsonPost.postJSONtoURL(MyApp.getServiceUrl() + "/Api/Story/GetStoryScrollContent", jsonSend);
                if(json == null)
                {
                    success = false;
                    message = "No response";
                }
                else {
                    try{

                        storyContent = json.getString("Content");
                        title = json.getString("Title");
                        synopsis = json.getString("Synopsis");

                    }
                    catch (JSONException e){
                        progressDialog.cancel();
                        success = false;
                        message = e.getMessage();
                    }

                }
                WriteStory.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        progressDialog.cancel();

                                    ed_storyContent.setText(Html.fromHtml(storyContent));
                                    ed_subTitle.setText(synopsis);
                                    ed_title.setText(title);
                    }
                });

            } catch (Exception e) {
                Log.e("log_tag", "Error parsing data " + e.toString());
                progressDialog.cancel();

            }
            progressDialog.cancel();
            return null;
        }
    }

    private class AutoSaveDraft
            extends AsyncTask {
        @Override
        protected Object doInBackground(Object... objects) {
            try {

                JSONStringer jsonSend = new JSONStringer()
                        .object()
                        .key("StoryContent").value(ed_storyContent.getTextHTML())
                        .key("Title").value(ed_title.getText().toString())
                        .key("Synopsis").value(ed_subTitle.getText().toString())
                        .endObject();
                JsonPost.postJSONtoURL(MyApp.getServiceUrl() + "/Api/Story/AutoSave", jsonSend);
            } catch (JSONException e) {
                Log.e("log_tag", "Error parsing data " + e.toString());
            }
            return null;
        }
    }
}