package com.RiddleBrothers.Poe.Settings;

import android.app.Application;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;

/**
 * Created with IntelliJ IDEA.
 * User: Mark Kamberger
 * Date: 10/13/12
 * Time: 3:44 PM
 * To change this template use File | Settings | File Templates.
 */
public class AppSettings extends Application {
    private static final DefaultHttpClient client = createClient();
    @Override
    public void onCreate(){
    }
    public static DefaultHttpClient getClient(){
        return client;
    }

    private static DefaultHttpClient createClient(){
        BasicHttpParams params = new BasicHttpParams();
        SchemeRegistry schemeRegistry = new SchemeRegistry();
        schemeRegistry.register(new Scheme("http", PlainSocketFactory.getSocketFactory(), 80));
        final SSLSocketFactory sslSocketFactory = SSLSocketFactory.getSocketFactory();
        schemeRegistry.register(new Scheme("https", sslSocketFactory, 443));
        ClientConnectionManager cm = new ThreadSafeClientConnManager(params, schemeRegistry);
        HttpConnectionParams.setConnectionTimeout(params,8000);
        DefaultHttpClient httpclient = new DefaultHttpClient(cm, params);

        return httpclient;
    }
}
