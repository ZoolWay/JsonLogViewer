# JsonLogViewer
Simple viewer for JSON-based logs in WPF. Currently supports one JSON object per line in file.
Will support different layouts later on.

The JSON log can come from any kind of log library.

# log4net Configuration
Here is a sample of a `log4net` rolling file appender using a JSON output format. It requires the `log4net.Ext.Json` extension.

```xml
<appender name="RollingFileJson" type="log4net.Appender.RollingFileAppender">
  <filter type="log4net.Filter.LoggerMatchFilter">
    <loggerToMatch value="MyNamespace.Core.DebugLog" />
    <acceptOnMatch value="false" />
  </filter>
  <file value="lastrun.json.log"/>
  <appendToFile value="false"/>
  <rollingStyle value="Size"/>
  <maxSizeRollBackups value="5"/>
  <maximumFileSize value="10MB"/>
  <layout type="log4net.Layout.SerializedLayout, log4net.Ext.Json">
    <decorator type="log4net.Ext.Json.Serializers.Newtonsoft.NewtonsoftDecorator, log4net.Ext.Json.Serializers.Newtonsoft" />
    <renderer type="log4net.ObjectRenderer.JsonObjectRenderer, log4net.Ext.Json">
      <factory type="log4net.Ext.Json.Serializers.Newtonsoft.NewtonsoftFactory, log4net.Ext.Json.Serializers.Newtonsoft" />
    </renderer>
    <default />
    <remove value="ndc" />
    <remove value="appname" />
    <!--
    <remove value="message" />
    <member value="message:messageobject" />
    -->
  </layout>
</appender>
```
