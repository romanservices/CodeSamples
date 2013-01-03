package com.RiddleBrothers.Poe.StoriesActivity;

import Models.StoryTitles;
import android.app.ListActivity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.Button;
import android.widget.ListView;
import com.RiddleBrothers.Poe.ArrayAdapters.StoryListArrayAdapter;
import com.RiddleBrothers.Poe.Helpers.EndlessScrollListener;
import com.RiddleBrothers.Poe.Helpers.INavBottom;
import com.RiddleBrothers.Poe.Helpers.ISortList;
import com.RiddleBrothers.Poe.MyActivities.MyCreatedStoryActivity;
import com.RiddleBrothers.Poe.MyActivities.MyOwnedStoryActivity;
import com.RiddleBrothers.Poe.MyActivity;
import com.RiddleBrothers.Poe.R;
import com.RiddleBrothers.Poe.SelectContactActivity;
import com.RiddleBrothers.Poe.Settings.MyApp;
import com.RiddleBrothers.Poe.StoryDetails.StoryDetailsActivity;
import com.RiddleBrothers.Poe.json.JsonPost;
import com.RiddleBrothers.Poe.json.LoginCheck;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.Stack;


public class StoryListActivity extends ListActivity implements ISortList, INavBottom {
    StoryListArrayAdapter storyListArrayAdapter;
    String _serviceUrl;
    List<StoryTitles> storyTitlesList;
    int _genre;
    int _currentPage;
    int _nextPage;
    boolean _hasNextPage;
    boolean _hasMorePages;
    boolean _srtRatToggle;
    boolean _srtAlToggle;
    ProgressDialog progressDialog;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.story_list_layout);
        Bundle bundle = this.getIntent().getExtras();
         _genre = bundle.getInt("genre");
        setTitle("Newest Stories");
        _nextPage = 1;
        _currentPage = 1;
        InitializeEvents();


        new GetTitles(false).execute();

            final ListView lv = getListView();
            lv.setTextFilterEnabled(true);
            //Click handler for listview
        lv.setOnItemClickListener(new OnItemClickListener() {
           public void onItemClick(AdapterView parent, View view, int position, long id) {
                StoryTitles story = getItem(position);
               ShowDetails(story.TitleID);
            }
        });
        lv.setOnScrollListener(new EndlessScrollListener(10) {
            @Override
            public void GetNextPage() {
               if(_hasNextPage)
                   new GetTitles(true).execute();
            }
        });
        }


    public void InitializeEvents()
    {
        InitButtons();
        InitUiElements();
        progressDialog = new ProgressDialog(this);
        progressDialog.setMessage("Fetching...");
        progressDialog.setIndeterminate(true);
        progressDialog.setCancelable(false);
        //
        _serviceUrl = ((MyApp) this.getApplication()).getServiceUrl();
        storyTitlesList = new Stack<StoryTitles>();
        storyListArrayAdapter = new StoryListArrayAdapter(getApplicationContext(), R.layout.story_list, storyTitlesList);

        //Sort Buttons


    }
    public StoryTitles getItem(int index) {
        return this.storyTitlesList.get(index);
    }

    public void ShowDetails(String storyID) {

        Intent intent = new Intent(StoryListActivity.this, StoryDetailsActivity.class);
        intent.putExtra("storyID", storyID);
        startActivity(intent);
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
               Intent intent = new Intent(StoryListActivity.this,MyActivity.class);
                startActivity(intent);
            }
        };
        storyTitlesList.clear();
        _nextPage = 1;
        storyTitlesList.clear();
        new GetTitles(false).execute();
    }

    @Override
    public void InitButtons() {
        Button _read;
        Button _write;
        Button _myStuff;


        _myStuff = (Button) findViewById(R.id.bottom_nav_btnMyContent);
        _myStuff.setText("Home");
        _myStuff.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(StoryListActivity.this, MyActivity.class);
                startActivity(intent);
            }
        });
        _read = (Button) findViewById(R.id.bottom_nav_btnRead);
        _read.setText("Read");
        _read.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(StoryListActivity.this, MyOwnedStoryActivity.class);
                startActivity(intent);
            }
        });

        _write = (Button) findViewById(R.id.bottom_nav_btnWrite);
        _write.setText("Write");
        _write.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(StoryListActivity.this, MyCreatedStoryActivity.class);
                startActivity(intent);
            }
        });
    }

    private class GetTitles extends AsyncTask {
        private boolean _isPage;
            private GetTitles(boolean isPage){
                _isPage = isPage;
                if(!_isPage)
                progressDialog.show();
            }
        @Override
        protected Object doInBackground(Object... objects) {
            try {

                JSONStringer jsonSend = new JSONStringer()
                        .object()
                        .key("Id").value(_genre )
                        .key("Page").value(_nextPage)
                        .endObject();
                if(_genre == 0){
                    jsonSend = new JSONStringer()
                            .object()
                            .key("Page").value(_nextPage)
                            .endObject();
                }
                JSONObject json = JsonPost.postJSONtoURL(_serviceUrl + "/Api/Story/GetStoriesByGenre", jsonSend);
                if (json == null) {
                    return null;
                }
                JSONArray story  = json.getJSONArray("Stories");

                _currentPage = json.getInt("CurrentPage");
                _hasMorePages = json.getBoolean("HasMorePages");
                _hasNextPage = json.getBoolean("HasNext");
                _nextPage = json.getInt("NextPage");
                for (int i = 0; i < story.length(); i++) {

                    JSONObject e = story.getJSONObject(i);
                    StoryTitles storyTitles = new StoryTitles();
                    storyTitles.StoryTitle = e.getString("StoryTitle");
                    storyTitles.CoverImageUrl = e.getString("CoverArtUrl");
                    storyTitles.SubTitle = e.getString("Synopsis");
                    storyTitles.Author = e.getString("Author");
                    storyTitles.AverageRating = e.getInt("AverageRating");
                    storyTitles.SortOrder = e.getInt("SortOrder");
                    storyTitles.ReviewCount = e.getInt("ReviewCount");
                    storyTitles.TitleID = e.getString("TitleID");
                    storyTitlesList.add(storyTitles);

                }
                StoryListActivity.this.runOnUiThread(new Runnable() {

                    @Override
                    public void run() {
                        if(!_isPage){
                        setListAdapter(storyListArrayAdapter);
                        progressDialog.cancel();
                        }
                        else {
                        storyListArrayAdapter.notifyDataSetChanged();
                        }
                    }
                });

            } catch (JSONException e) {
                Log.e("log_tag", "Error parsing data " + e.toString());
            }

            return null;
        }
    }



    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.mainmenu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.mm_MyStories:

                break;
            case R.id.mm_NewTitle:

                break;
            case R.id.mm_Invite:
                Intent intentContacts = new Intent(this, SelectContactActivity.class);
                startActivity(intentContacts);

                break;
        }
        return true;
    }

    @Override
     public void SortByRating()
     {
         Collections.sort(storyTitlesList, new Comparator<StoryTitles>() {
             @Override
             public int compare(StoryTitles storyTitles, StoryTitles storyTitles1) {
                 Integer a = storyTitles.SortOrder;
                 Integer b = storyTitles1.SortOrder;
                 return a.compareTo(b);
             }
             @Override
             public boolean equals(Object o) {
                 return false;  //To change body of implemented methods use File | Settings | File Templates.
             }
         });
         if(_srtRatToggle)   {
             Collections.reverse(storyTitlesList);

         }
         storyListArrayAdapter.notifyDataSetChanged();
         }

    @Override
    public void InitUiElements() {
        Button _sortAlpha;
        Button _sortRating;
        _sortAlpha = (Button)findViewById(R.id.story_list_srtAlpha);
        _sortAlpha.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                _srtAlToggle = !_srtAlToggle;
                SortByTitle();
            }
        });
        _sortRating = (Button) findViewById(R.id.story_list_srtRating);
        _sortRating.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                _srtRatToggle = !_srtRatToggle;
                SortByRating();
            }
        });
    }

    @Override
    public void SortByTitle()
    {
           Collections.sort(storyTitlesList, new Comparator<StoryTitles>() {
               @Override
               public int compare(StoryTitles storyTitles, StoryTitles storyTitles1) {
                   String a = storyTitles.StoryTitle.substring(0,1);
                   String b = storyTitles1.StoryTitle.substring(0,1);
                   return a.compareToIgnoreCase(b);
               }

               @Override
               public boolean equals(Object o) {
                   return false;  //To change body of implemented methods use File | Settings | File Templates.
               }
           });
        if(_srtAlToggle)
           Collections.reverse(storyTitlesList);


        storyListArrayAdapter.notifyDataSetChanged();

    }


}
