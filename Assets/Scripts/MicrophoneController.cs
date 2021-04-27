using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneController : MonoBehaviour
{
    public float RmsValue { get { return _rmsValue; } }
    public float DbValue { get { return _dbValue; } }
    public float PitchValue { get { return _pitchValue; } }

    [SerializeField]
    private float _rmsValue;
    [SerializeField]
    private float _dbValue;
    [SerializeField]
    private float _pitchValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    private AudioSource audioSource;

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 2599, 44100);
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    private void Update()
    {
        AnalyzeSound();
    }

    public void AnalyzeSound()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        _rmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        _dbValue = 20 * Mathf.Log10(_rmsValue / RefValue); // calculate dB
        if (_dbValue < -160) _dbValue = -160; // clamp it to -160dB min
                                              // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        _pitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
    }
}