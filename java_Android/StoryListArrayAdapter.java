package com.RiddleBrothers.Poe.ArrayAdapters;

import Models.StoryTitles;
import android.content.Context;
import android.content.res.Resources;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.RatingBar;
import android.widget.TextView;
import com.RiddleBrothers.Poe.Helpers.DownLoadImage;
import com.RiddleBrothers.Poe.R;

import java.util.ArrayList;
import java.util.List;

/**
 * Created with IntelliJ IDEA.
 * User: Mark Kamberger
 * Date: 10/16/12
 * Time: 11:34 AM
 * To change this template use File | Settings | File Templates.
 */
public class StoryListArrayAdapter extends ArrayAdapter<StoryTitles>  {
    private static final String tag = "StoryListArrayAdapter";
    private static final String ASSETS_DIR = "images/";
    private Context context;


    private ImageView _coverArt;
    private TextView _title;
    private TextView _author;
    private TextView _synopsis;
    private TextView _reviewData;
    private RatingBar _ratingBar;

    private List<StoryTitles> storyTitles = new ArrayList<StoryTitles>();
    public StoryListArrayAdapter(Context context, int textViewResourceId,
                                 List<StoryTitles> objects) {
        super(context, textViewResourceId, objects);
        this.context = context;
        this.storyTitles = objects;
    }

    public int getCount() {
        return this.storyTitles.size();
    }
    public StoryTitles getItem(int index) {
        return this.storyTitles.get(index);
    }
    public View getView(int position, View convertView, ViewGroup parent) {
        View row = convertView;
        if (row == null) {
            // ROW INFLATION
            Log.d(tag, "Starting XML Row Inflation ... ");
            LayoutInflater inflater = (LayoutInflater) this.getContext()
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            row = inflater.inflate(R.layout.story_list, parent, false);
            Log.d(tag, "Successfully completed XML Row Inflation!");
        }

        // Get item
        StoryTitles storyTitles1 = getItem(position);
        Resources res = this.getContext().getResources();

        _coverArt = (ImageView) row.findViewById(R.id.story_list_CoverArt);
        _title = (TextView) row.findViewById(R.id.story_list_Title);
        _author = (TextView) row.findViewById(R.id.story_list_Author);
        _synopsis = (TextView) row.findViewById(R.id.story_list_Synopsis);
        _ratingBar = (RatingBar) row.findViewById(R.id.story_list_RatingBar);
        _reviewData = (TextView) row.findViewById(R.id.story_list_ReviewData);
        //
        _title.setText(storyTitles1.StoryTitle);
        _title.setClickable(false);
        _title.setFocusable(false);
        //
        _author.setText("By "+storyTitles1.Author);
        _author.setClickable(false);
        _author.setFocusable(false);
        //
        _synopsis.setText(storyTitles1.SubTitle);
        _synopsis.setClickable(false);
        _synopsis.setFocusable(false);
        //
        _ratingBar.setRating(storyTitles1.AverageRating);
        _ratingBar.setClickable(false);
        _ratingBar.setFocusable(false);
        //
        _reviewData.setText(String.valueOf("Rated: "
                + storyTitles1.AverageRating
                + "/5  (" + storyTitles1.ReviewCount + ")")
                + " reviews and  ("
                + String.valueOf(storyTitles1.PurchasedCount)
                + ") readers"
                + "  Votes: "
                + storyTitles1.VoteCount);
        _reviewData.setClickable(false);
        _reviewData.setFocusable(false);
        //

        new DownLoadImage(_coverArt)
                .execute(storyTitles1.CoverImageUrl.toString()+"&width=75&height=75");


        return row;
    }






}
