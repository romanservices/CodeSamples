package com.RiddleBrothers.Poe.WriteActivities;

import android.content.Context;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.text.Editable;
import android.text.Html;
import android.text.style.AlignmentSpan;
import android.text.style.StyleSpan;
import android.text.style.UnderlineSpan;
import android.util.AttributeSet;
import android.view.ActionMode;
import android.view.Gravity;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.EditText;
import com.RiddleBrothers.Poe.R;

/**
 * Created with IntelliJ IDEA.
 * User: Mark Kamberger
 * Date: 11/28/12
 * Time: 3:00 PM
 * To change this template use File | Settings | File Templates.
 */
public class TextEditor extends EditText {

    private Drawable imgCloseButton = getResources().getDrawable(R.drawable.agressive);

    public TextEditor(Context context) {
        super(context);
        init();
    }

    public TextEditor(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    public TextEditor(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
        init();
    }

    public String getTextHTML() {
        return Html.toHtml(this.getText());
    }

    public String getTextString(){
        return this.getText().toString();
    }
    private void init() {
        this.setSingleLine(false);
        this.setGravity(Gravity.TOP);
        this.setCustomSelectionActionModeCallback(new ActionMode.Callback() {

            public boolean onPrepareActionMode(ActionMode mode, Menu menu) {
                return false;
            }

            public void onDestroyActionMode(ActionMode mode) {
            }

            public boolean onCreateActionMode(ActionMode mode, Menu menu) {
                menu.clear();
                MenuItem bold = menu.add(0, 1, 0, "bold");
                MenuItem ul = menu.add(0, 2, 1, "ul");
                MenuItem italic = menu.add(0, 3, 2, "italic");
                MenuItem remove = menu.add(0, 4, 3, "remove");

                //
                //set icons
                //
                bold.setIcon(R.drawable.font_bold_icon_w);
                ul.setIcon(R.drawable.font_underline_icon_w);
                italic.setIcon(R.drawable.font_italic_icon_w);
                remove.setIcon(R.drawable.delete_icon);

                //
                //set showAction
                //
                bold.setShowAsAction(MenuItem.SHOW_AS_ACTION_ALWAYS);
                ul.setShowAsAction(MenuItem.SHOW_AS_ACTION_ALWAYS);
                italic.setShowAsAction(MenuItem.SHOW_AS_ACTION_ALWAYS);
                remove.setShowAsAction(MenuItem.SHOW_AS_ACTION_ALWAYS);

                return true;
            }

            public boolean onActionItemClicked(ActionMode mode, MenuItem item) {
                switch (item.getItemId()) {
                    case 1:
                        Bold();
                        return true;
                    case 2:
                        UnderLine();
                        return true;
                    case 3:
                        Italic();
                        return true;
                    case 4:
                        Remove();
                        return true;

                }
                return false;
            }
        });
    }

    private void Bold(){
        Editable et = this.getText();
        String selected = this.getText().toString().substring(this.getSelectionStart(), this.getSelectionEnd());
        et.replace(this.getSelectionStart(),this.getSelectionEnd(),Html.fromHtml("<b>"+selected+"</b>"));
    }
    private void Italic(){
        Editable et = this.getText();
        String selected = this.getText().toString().substring(this.getSelectionStart(), this.getSelectionEnd());
        et.replace(this.getSelectionStart(),this.getSelectionEnd(),Html.fromHtml("<i>" + selected + "</i>"));
    }
    private void UnderLine(){
        Editable et = this.getText();
        String selected = this.getText().toString().substring(this.getSelectionStart(), this.getSelectionEnd());
        et.replace(this.getSelectionStart(),this.getSelectionEnd(),Html.fromHtml("<u>"+selected+"</u>"));
    }


    private void Remove(){

        int start = this.getSelectionStart();
        int end = this.getSelectionEnd();
        if (start > end) {
            int temp = end;
            end = start;
            start = temp;
        }
        StyleSpan[] styleSpans;
        //Remove Bold/Italic
        styleSpans = this.getText().getSpans(start, end, StyleSpan.class);
        for (int i = 0; i < styleSpans.length; i++) {
            if (styleSpans[i].getStyle() == Typeface.BOLD
                    || styleSpans[i].getStyle() == Typeface.ITALIC) {
                this.getText().removeSpan(styleSpans[i]);

            }
        }
        //Remove Underline
        UnderlineSpan[] underSpan = this.getText().getSpans(start, end, UnderlineSpan.class);
        for (int i = 0; i < underSpan.length; i++) {
            this.getText().removeSpan(underSpan[i]);

        }
        //Remove Alignment
        AlignmentSpan[] alignSpan = this.getText().getSpans(start,end,AlignmentSpan.class);
        for (int i = 0; i < underSpan.length; i++) {
            this.getText().removeSpan(alignSpan[i]);

        }

    }

}
